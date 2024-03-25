namespace VacationPlanner
{
    // In a new file, e.g., Extensions/ClaimsPrincipalExtensions.cs

    using System;
    using System.Security.Claims;

    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.FindFirstValue(ClaimTypes.NameIdentifier); // or the claim type you use for the user's ID
        }
    }

}
