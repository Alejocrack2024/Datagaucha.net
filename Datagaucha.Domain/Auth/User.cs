/* Copyright (c) 2026 David-irusta
    This code is licensed under the MIT License (see LICENSE file for details) */

using Datagaucha.Domain.FileSystem;
using System.Security.Cryptography;
using System.Text;

namespace Datagaucha.Domain.Auth;

public class User : Person
{
    private string userName = "";

    public string UserName { get => userName; set => userName = value; }

    private string password = "";

    public string Password { get => password; set => password = value; }

    private DateTime? dateBirth = null;

    public DateTime? DateBirth { get => dateBirth; set => dateBirth = value; }

    private Image? image = null;

    public virtual Image? Image { get => image; set => image = value; }

    private static string encrypt(string password)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(password);
        byte[] byteHash = SHA1.HashData(bytes);

	    return Convert.ToBase64String(byteHash);
    }

    public virtual void SetPassword(string password)
    {
	    this.password = User.encrypt(password);
    }

    public virtual bool IsPassword(string password)
    {
	    string  encryptedPassword= User.encrypt(password);
	    if (this.password == encryptedPassword)
	    {
		    return true;
	    }

	    return false;
    }

    public string? GetAvatarurl()
    {
        if (this.Image != null)
        {
            return "api/images/" + this.Image.FileName;
        }

        return null;
    }
}
