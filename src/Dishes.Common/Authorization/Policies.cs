namespace Dishes.Common.Authorization;

public static class Policies
{
  public const string RequireAdminFromNigeria = nameof(RequireAdminFromNigeria);

  public static class Roles
  {
    public const string Admin = "admin";
  }
}