using AI.Nova.Shared.Features.Identity;
using AI.Nova.Shared.Features.Identity.Dtos;
using AI.Nova.Client.Core.Infrastructure.Services.DiagnosticLog;
using AI.Nova.Shared.Features.Diagnostic;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace AI.Nova.Client.Core.Components.Pages.Management;

public partial class UsersPage
{
    private UserDto selectedUserDto = new();


    private bool isLoadingUsers;
    private int? onlineUsersCount;
    private string? loadingUserKey;
    private string? userSearchText;
    private string? sessionSearchText;
    private bool isDeleteUserDialogOpen;
    private UserDto? selectedUserItem;
    private bool isLoadingOnlineUsersCount;
    private bool isRevokeAllUserSessionsDialogOpen;
    private CancellationTokenSource? loadRoleDataCts;
    private List<UserSessionDto> allUserSessions = [];
    private List<UserSessionDto> filteredUserSessions = [];

    private BitDataGrid<UserDto>? dataGrid;
    private BitDataGridItemsProvider<UserDto> usersProvider = default!;
    private bool allItemsLoaded;


    [AutoInject] IUserManagementController userManagementController = default!;
    [AutoInject] HubConnection hubConnection = default!;

    protected override async Task OnInitAsync()
    {
        await base.OnInitAsync();

        PrepareGridDataProvider();

        await LoadOnlineUsersCount();
    }


    private void PrepareGridDataProvider()
    {
        usersProvider = async req =>
        {
            try
            {
                isLoadingUsers = true;
                allItemsLoaded = false;

                var query = new ODataQuery
                {
                    Top = req.Count ?? 10,
                    Skip = req.StartIndex,
                };

                if (string.IsNullOrEmpty(userSearchText) is false)
                {
                    var search = userSearchText.ToLower();
                    query.Filter = $"(contains(tolower({nameof(UserDto.FullName)}),'{search}') or " +
                                   $"contains(tolower({nameof(UserDto.Email)}),'{search}') or " +
                                   $"contains(tolower({nameof(UserDto.UserName)}),'{search}') or " +
                                   $"contains(tolower({nameof(UserDto.PhoneNumber)}),'{search}'))";
                }

                var data = await userManagementController.WithQuery(query.ToString()).GetUsers(req.CancellationToken);

                var items = data!.Items!;
                var totalCount = (int)data!.TotalCount;

                allItemsLoaded = totalCount > 0 && req.StartIndex + items.Length >= totalCount;

                return BitDataGridItemsProviderResult.From(items, totalCount);
            }
            catch (OperationCanceledException)
            {
                return default;
            }
            catch (Exception exp)
            {
                ExceptionHandler.Handle(exp);
                return BitDataGridItemsProviderResult.From(Array.Empty<UserDto>(), 0);
            }
            finally
            {
                isLoadingUsers = false;
            }
        };
    }


    private async Task RefreshData()
    {
        await Task.WhenAll(
            dataGrid!.RefreshDataAsync(),
            LoadOnlineUsersCount()
        );
    }

    private async Task LoadOnlineUsersCount()
    {
        if (isLoadingOnlineUsersCount) return;

        try
        {
            isLoadingOnlineUsersCount = true;
            onlineUsersCount = await userManagementController.GetOnlineUsersCount(CurrentCancellationToken);
        }
        finally
        {
            isLoadingOnlineUsersCount = false;
        }
    }

    private async Task DeleteUser()
    {
        if (selectedUserItem is null) return;

        if (await AuthManager.TryEnterElevatedAccessMode(CurrentCancellationToken) is false) return;

        await userManagementController.Delete(selectedUserItem.Id, CurrentCancellationToken);

        await RefreshData();
    }

    private async Task HandleOnSelectUser(UserDto? user)
    {
        if (user is null) return;

        try
        {
            if (loadRoleDataCts is not null)
            {
                using var currentCts = loadRoleDataCts;
                loadRoleDataCts = new();

                await currentCts.TryCancel();
            }

            loadRoleDataCts = new();

            loadingUserKey = user.Id.ToString();
            selectedUserItem = user;

            user.Patch(selectedUserDto);

            allUserSessions = await userManagementController.GetUserSessions(user.Id, CurrentCancellationToken);

            SearchSessions();
        }
        finally
        {
            if (loadingUserKey == user.Id.ToString())
            {
                loadingUserKey = null;
            }
        }
    }

    private async Task RevokeUserSession(UserSessionDto session)
    {
        if (selectedUserItem is null) return;

        if (await AuthManager.TryEnterElevatedAccessMode(CurrentCancellationToken) is false) return;

        await userManagementController.RevokeUserSession(session.Id, CurrentCancellationToken);

        await HandleOnSelectUser(selectedUserItem);
    }
    private async Task RevokeAllSessions()
    {
        if (selectedUserItem is null) return;

        if (await AuthManager.TryEnterElevatedAccessMode(CurrentCancellationToken) is false) return;

        await userManagementController.RevokeAllUserSessions(selectedUserItem.Id, CurrentCancellationToken);

        await HandleOnSelectUser(selectedUserItem);
    }

    private async Task SearchUsers()
    {
        await dataGrid!.RefreshDataAsync();
    }

    private void SearchSessions()
    {
        filteredUserSessions = allUserSessions;

        if (string.IsNullOrWhiteSpace(sessionSearchText) is false)
        {
            var t = sessionSearchText.Trim();
            filteredUserSessions = [.. allUserSessions.Where(us => ((us.IP + us.Address + us.DeviceInfo + us.RenewedOnDateTimeOffset + us.Id) ?? string.Empty).Contains(t, StringComparison.InvariantCultureIgnoreCase))];
        }
    }

    /// <summary>
    /// <inheritdoc cref="SharedAppMessages.UPLOAD_DIAGNOSTIC_LOGGER_STORE"/>
    /// </summary>
    private async Task ReadUserSessionLogs(Guid userSessionId)
    {
        var logs = await hubConnection.InvokeAsync<DiagnosticLogDto[]>(SharedAppMessages.GetUserSessionLogs, userSessionId);

        DiagnosticLogger.Store.Clear();
        foreach (var log in logs)
        {
            DiagnosticLogger.Store.Enqueue(log);
        }

        PubSubService.Publish(ClientAppMessages.SHOW_DIAGNOSTIC_MODAL);
    }
}
