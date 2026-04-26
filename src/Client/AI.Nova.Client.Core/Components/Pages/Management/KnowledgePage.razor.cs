using AI.Nova.Shared.Features.Knowledge;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Bit.BlazorUI;
using Microsoft.AspNetCore.Components.Forms;

namespace AI.Nova.Client.Core.Components.Pages.Management;

public partial class KnowledgePage
{
    [AutoInject] private IKnowledgeController knowledgeController = default!;
    [AutoInject] private NavigationManager NavigationManager = default!;

    private BitDataGrid<KnowledgeBaseDto>? dataGrid;
    private BitDataGrid<KnowledgeDocumentDto>? docGrid;
    private BitDataGridItemsProvider<KnowledgeBaseDto>? kbProvider;
    private BitDataGridItemsProvider<KnowledgeDocumentDto>? docProvider;
    private EditForm? editForm;
    private AppDataAnnotationsValidator? validatorRef;
    private bool isSaving;
    private bool isChanged;

    private bool isLoading;
    private string? kbSearchText;
    private string? docSearchText;
    private KnowledgeBaseDto? selectedKnowledgeBase;
    private KnowledgeBaseDto newKnowledgeBase = new();
    private bool isAddKnowledgeBaseDialogOpen;
    private bool isDeleteKnowledgeBaseDialogOpen;
    private bool isDocumentDetailDialogOpen;
    private KnowledgeDocumentDto? selectedDocument;
    private BitDataGrid<KnowledgeDocumentChunkDto>? chunkGrid;
    private BitDataGridItemsProvider<KnowledgeDocumentChunkDto>? chunkProvider;

    private string? recallSearchText;
    private string? recallDocumentName;
    private double vectorWeight = 0.85;
    private BitDataGrid<KnowledgeDocumentChunkDto>? recallGrid;
    private BitDataGridItemsProvider<KnowledgeDocumentChunkDto>? recallProvider;



    protected override async Task OnInitAsync()
    {
        kbProvider = async req =>
        {
            isLoading = true;
            try
            {
                var query = new ODataQuery
                {
                    Top = req.Count ?? 50,
                    Skip = req.StartIndex,
                };

                if (string.IsNullOrWhiteSpace(kbSearchText) is false)
                {
                    var search = kbSearchText.ToLower();
                    query.Filter = $"contains(tolower({nameof(KnowledgeBaseDto.Name)}),'{search}') or contains(tolower({nameof(KnowledgeBaseDto.Description)}),'{search}')";
                }

                var data = await knowledgeController.WithQuery(query.ToString()).GetKnowledgeBases(req.CancellationToken);

                return BitDataGridItemsProviderResult.From(data!.Items!.ToList(), (int)data.TotalCount);
            }
            catch (OperationCanceledException) { return default; }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
                return BitDataGridItemsProviderResult.From(new List<KnowledgeBaseDto>(), 0);
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }
        };

        await base.OnInitAsync();
    }

    private async Task RefreshKnowledgeBases()
    {
        if (dataGrid is not null)
            await dataGrid.RefreshDataAsync();
    }

    private async Task SearchKnowledgeBases()
    {
        await RefreshKnowledgeBases();
    }

    private async Task HandleOnSelectKnowledgeBase(KnowledgeBaseDto item)
    {
        selectedKnowledgeBase = item;
        docProvider = async req =>
        {
            try
            {
                var query = new ODataQuery
                {
                    Top = req.Count ?? 50,
                    Skip = req.StartIndex,
                    Filter = $"{nameof(KnowledgeDocumentDto.KnowledgeBaseId)} eq {selectedKnowledgeBase.Id}"
                };

                if (string.IsNullOrWhiteSpace(docSearchText) is false)
                {
                    var search = docSearchText.ToLower();
                    query.Filter += $" and contains(tolower({nameof(KnowledgeDocumentDto.Title)}),'{search}')";
                }

                var data = await knowledgeController.WithQuery(query.ToString()).GetDocuments(req.CancellationToken);

                return BitDataGridItemsProviderResult.From(data!.Items!.ToList(), (int)data.TotalCount);
            }
            catch (OperationCanceledException) { return default; }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
                return BitDataGridItemsProviderResult.From(new List<KnowledgeDocumentDto>(), 0);
            }
        };

        recallProvider = async req =>
        {
            if (string.IsNullOrWhiteSpace(recallSearchText))
                return BitDataGridItemsProviderResult.From(new List<KnowledgeDocumentChunkDto>(), 0);

            try
            {
                var data = await knowledgeController.SearchChunks(selectedKnowledgeBase!.Id, recallSearchText, vectorWeight, recallDocumentName, req.CancellationToken);
                
                var items = data!.Items!.AsEnumerable();

                // 内存重排序逻辑
                if (req.SortByColumn is not null)
                {
                    var col = req.SortByColumn;
                    var direction = req.SortByAscending;

                    // 根据字段名进行动态匹配（注意：此处手动匹配属性，确度更高）
                    Func<KnowledgeDocumentChunkDto, object?> keySelector = col.Title switch
                    {
                        "Score" => c => c.Score,
                        "#" => c => c.Index,
                        _ when col.Title == Localizer[nameof(AppStrings.Document)] => c => c.DocumentTitle,
                        _ => c => c.Score
                    };

                    items = direction
                        ? items.OrderBy(keySelector)
                        : items.OrderByDescending(keySelector);
                }
                else
                {
                    // 缺省逻辑：按 Score 降序
                    items = items.OrderByDescending(c => c.Score);
                }

                return BitDataGridItemsProviderResult.From(items.ToList(), (int)data.TotalCount);
            }
            catch (OperationCanceledException) { return default; }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
                return BitDataGridItemsProviderResult.From(new List<KnowledgeDocumentChunkDto>(), 0);
            }
        };

        await RefreshDocuments();
    }

    private async Task RefreshDocuments()
    {
        if (docGrid is not null)
            await docGrid.RefreshDataAsync();
    }

    private async Task SearchDocuments()
    {
        await RefreshDocuments();
    }

    private async Task SearchRecall()
    {
        if (recallGrid is not null)
            await recallGrid.RefreshDataAsync();
    }

    private HashSet<Guid> expandedChunkIds = [];

    private void ToggleChunkExpand(Guid id)
    {
        if (expandedChunkIds.Contains(id))
        {
            expandedChunkIds.Clear();
        }
        else
        {
            expandedChunkIds.Clear();
            expandedChunkIds.Add(id);
        }
    }

    private void ShowAddKnowledgeBaseDialog()
    {
        newKnowledgeBase = new();
        isAddKnowledgeBaseDialogOpen = true;
    }

    private async Task AddKnowledgeBase()
    {
        await knowledgeController.CreateBase(newKnowledgeBase, CurrentCancellationToken);
        isAddKnowledgeBaseDialogOpen = false;
        await RefreshKnowledgeBases();
    }

    private async Task UpdateKnowledgeBase()
    {
        if (selectedKnowledgeBase is not null)
        {
            await knowledgeController.UpdateBase(selectedKnowledgeBase, CurrentCancellationToken);
            await RefreshKnowledgeBases();
        }
    }

    private async Task DeleteKnowledgeBase()
    {
        if (selectedKnowledgeBase is not null)
        {
            await knowledgeController.DeleteBase(selectedKnowledgeBase.Id, CurrentCancellationToken);
            selectedKnowledgeBase = null;
            isDeleteKnowledgeBaseDialogOpen = false;
            await RefreshKnowledgeBases();
        }
    }

    private bool isAddDocumentDialogOpen;
    private BitFileUpload fileUploadRef = default!;

    private async Task ShowAddDocumentDialog()
    {
        isAddDocumentDialogOpen = true;
        await Task.CompletedTask;
    }

    private async Task HandleOnDocumentUploadComplete()
    {
        await RefreshDocuments();
        SnackBarService.Success(Localizer[nameof(AppStrings.FileUploadedSuccessfully)]);
    }

    private async Task HandleOnDocumentUploadFailed(BitFileInfo fileInfo)
    {
        SnackBarService.Error(string.IsNullOrEmpty(fileInfo.Message) ? Localizer[nameof(AppStrings.FileUploadFailed)] : fileInfo.Message);
        await Task.CompletedTask;
    }

    private async Task<string> GetUploadDocumentUrl()
    {
        var uploadUrl = new Uri(AbsoluteServerAddress, $"/api/v1/Knowledge/UploadDocument/{selectedKnowledgeBase!.Id}").ToString();

        return uploadUrl;
    }

    private async Task<Dictionary<string, string>> GetUploadRequestHeaders()
    {
        var accessToken = await AuthManager.GetFreshAccessToken(requestedBy: nameof(BitFileUpload));

        return new() { { "Authorization", $"Bearer {accessToken}" } };
    }

    private async Task DeleteDocument(KnowledgeDocumentDto doc)
    {
        await knowledgeController.DeleteDocument(doc.Id, CurrentCancellationToken);
        await RefreshDocuments();
    }

    private async Task ShowDocumentDetailDialog(KnowledgeDocumentDto doc)
    {
        selectedDocument = doc;
        chunkProvider = async req =>
        {
            try
            {
                var query = new ODataQuery
                {
                    Top = req.Count ?? 50,
                    Skip = req.StartIndex,
                    Filter = $"{nameof(KnowledgeDocumentChunkDto.DocumentId)} eq {selectedDocument.Id}",
                    OrderBy = nameof(KnowledgeDocumentChunkDto.Index)
                };

                var data = await knowledgeController.WithQuery(query.ToString()).GetChunks(req.CancellationToken);

                return BitDataGridItemsProviderResult.From(data!.Items!.ToList(), (int)data.TotalCount);
            }
            catch (OperationCanceledException) { return default; }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
                return BitDataGridItemsProviderResult.From(new List<KnowledgeDocumentChunkDto>(), 0);
            }
        };
        isDocumentDetailDialogOpen = true;
        await Task.CompletedTask;
    }

    private async Task DownloadDocument(KnowledgeDocumentDto doc)
    {
        var url = new Uri(AbsoluteServerAddress, $"/api/v1/Knowledge/DownloadDocument/{doc.Id}").ToString();
        NavigationManager.NavigateTo(url, forceLoad: true);
        await Task.CompletedTask;
    }
}
