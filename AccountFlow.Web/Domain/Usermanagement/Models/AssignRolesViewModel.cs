using System;

namespace AccountFlow.Web.Domain.Usermanagement.Models;

public class AssignRolesViewModel
{
    public List<UserViewModel> Users { get; set; }
    public List<string> AvailableRoles { get; set; }
}
