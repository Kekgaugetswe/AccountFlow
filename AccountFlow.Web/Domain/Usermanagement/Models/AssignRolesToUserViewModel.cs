using System;

namespace AccountFlow.Web.Domain.Usermanagement.Models;

public class AssignRolesToUserViewModel
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public List<string> AvailableRoles { get; set; } = new();
    public List<string> SelectedRoles { get; set; } = new();

}
