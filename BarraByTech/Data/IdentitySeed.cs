using Microsoft.AspNetCore.Identity;

public static class IdentitySeed
{
    public static async Task SeedAdminUserAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        string adminEmail = "admin@gmail.com";
        string adminPassword = "Admin@123";
        string adminRole = "Admin";

        // Criar o role Admin se não existir
        if (!await roleManager.RoleExistsAsync(adminRole))
        {
            await roleManager.CreateAsync(new IdentityRole(adminRole));
        }

        // Verificar se o usuário já existe
        var existingUser = await userManager.FindByEmailAsync(adminEmail);
        if (existingUser == null)
        {
            var adminUser = new IdentityUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            // Criar usuário
            var result = await userManager.CreateAsync(adminUser, adminPassword);

            // Adicionar ao role Admin
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, adminRole);
            }
        }
    }
}