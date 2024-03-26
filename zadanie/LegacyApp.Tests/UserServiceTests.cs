namespace LegacyApp.Tests;

public class UserServiceTests
{
    [Fact]
    public void AddUser_ReturnsFalseWhenFirstNameIsEmpty()
    {
        //Arrange
        var userService = new UserService();

        // Act
        var result = userService.AddUser(
            null,
            "Kowalski",
            "nullkowalski@mail.com",
            DateTime.Parse("2000-01-01"), 
            1);

        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void AddUser_ThrowsExceptionWhenClientDoesNotExist()
    {
        //Arrange
        var userService = new UserService();

        // Act
        var action = () => {
            var result = userService.AddUser(
                "Jan",
                "Kowalski",
                "jankowalski@mail.com",
                DateTime.Parse("2000-01-01"), 
                100);
        };
        
        // Assert
        Assert.Throws<ArgumentException>(action);
    }
}