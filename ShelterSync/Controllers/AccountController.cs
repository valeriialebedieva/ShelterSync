using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ShelterHelper.Controllers;

/// <summary>
/// Controller for managing user authentication (login, logout, access denied).
/// Handles user sign-in and sign-out operations using cookie-based authentication.
/// </summary>
public class AccountController : Controller
{
    /// <summary>
    /// Displays the login page.
    /// </summary>
    /// <returns>Login view</returns>
    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    /// <summary>
    /// Handles user login with username/password.
    /// In a production app, this would validate against a database.
    /// For demo purposes, accepts any non-empty credentials.
    /// </summary>
    /// <param name="username">User's username</param>
    /// <param name="password">User's password</param>
    /// <param name="returnUrl">URL to redirect to after successful login</param>
    /// <returns>Redirects to home or specified return URL on success</returns>
    [HttpPost]
    public async Task<IActionResult> Login(string username, string password, string? returnUrl = null)
    {
        // Basic validation
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            ModelState.AddModelError("", "Username and password are required");
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // Demo: Simple validation (in production, validate against database with hashed passwords)
        // For this demo, any non-empty credentials are accepted
        if (username.Length > 0 && password.Length >= 6)
        {
            // Create claims for the user
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier, username),
                new Claim("IsAdmin", username.Equals("admin", StringComparison.OrdinalIgnoreCase) ? "true" : "false")
            };

            // Create identity and principal
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
            };

            // Sign in user
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            // Redirect to return URL or home
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        // Invalid credentials
        ModelState.AddModelError("", "Invalid username or password (password must be at least 6 characters)");
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    /// <summary>
    /// Logs out the current user.
    /// </summary>
    /// <returns>Redirects to home page</returns>
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    /// <summary>
    /// Displays access denied message when user lacks required permissions.
    /// </summary>
    /// <returns>Access denied view</returns>
    public IActionResult AccessDenied()
    {
        return View();
    }
}