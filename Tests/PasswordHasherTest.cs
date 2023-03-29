using DAL.Entities;
using DAL;
using NUnit.Framework;

namespace Tests;

public class PasswordHasherTest : BaseUnitTests
{
    User Viktoria = new User();
    [SetUp]
    public void Setup()
    {
        Viktoria = new User("test@mail.com","Viktoria","Test","TestPW123");
        Viktoria.HashedPassword = PasswordHasher.HashPasword(Viktoria.Password, out var saltViktoria);
        Viktoria.Salt = saltViktoria;
    }

    [Test]
    public void LoginSuccessfullTest()
    {
        //Login should work since correct password is handed over
        try 
        {
            var passwordEntered = "TestPW123";
            bool VerifiedViktoria = PasswordHasher.VerifyPassword(passwordEntered, Viktoria.HashedPassword, Viktoria.Salt);

            Assert.IsTrue(VerifiedViktoria);
        }
        catch(InvalidCastException e)
        {
            Console.WriteLine(e);
        }
    }

    [Test]
    public void LoginFailTest()
    {
        //Login should not work since Password is not correct
        try
        {
            var passwordEnteredWrong = "TestPW1234";
            bool VerifiedViktoria = PasswordHasher.VerifyPassword(passwordEnteredWrong, Viktoria.HashedPassword, Viktoria.Salt);

            Assert.IsFalse(VerifiedViktoria);
        }
        catch(InvalidCastException e)
        {
            Console.WriteLine(e);
        }

    }

  
}
/*Notizen
Viktoria.HashedPassword = PasswordHasher.HashPasword(Viktoria.Password, out var saltViktoria);
byte[] saltOfViktoria = saltViktoria;
Console.WriteLine($"Generated salt: {Convert.ToHexString(saltViktoria)}");
Console.WriteLine($"Password hash: {Viktoria.HashedPassword}");  

Zur überprüfung des Salt und des Hash Werts habe ich beide Ausgegeben und verglichen, 
Ein zweiter User mit dem gleichen Passwort bekommt nicht den gleichen Hash wert und auch nicht den gleichen Salt wert 

*/