using Catalogue.Api.Controllers;
using Catalogue.Api.Dtos;
using Catalogue.Api.Models;
using Catalogue.Api.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Catalogue.UnitTests;

public class ItemsControllerTests
{

    private readonly Mock<IItemRepository> repositoryMock = new();
    private readonly Mock<ILogger<ItemsController>> loggerMock = new();
    private Random rand = new();
    private Item CreateRandomItem()
    {
        return new()
        {
            Id = Guid.NewGuid(),
            Name = Guid.NewGuid().ToString(),
            Price = rand.Next(1000),
            CreatedDate = DateTimeOffset.Now,
        };

    }

    [Fact]
    public async void GetItem_UnexistingItem_ReturnsNotFound()
    {
        // Arrange
        repositoryMock.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Item)null);

        var controller = new ItemsController(repositoryMock.Object, loggerMock.Object);

        // Act
        var result = await controller.GetItem(It.IsAny<Guid>());
        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
        //result.Result.Should().BeOfType <NotFoundResult>(); 
    }

    [Fact]
    public async void GetItem_ExistingItem_ReturnsItem()
    {
        var expectedItem = CreateRandomItem();
        repositoryMock.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
            .ReturnsAsync(expectedItem);
        var controller = new ItemsController(repositoryMock.Object, loggerMock.Object);

        var result = await controller.GetItem(Guid.NewGuid());

        repositoryMock.Verify(mock => mock.GetItemAsync(It.IsAny<Guid>()), Times.Once());
        result.Value.Should().BeEquivalentTo(expectedItem, options => options.ComparingByMembers<Item>());

        // * can use below as they are records and already compare via properties not reference
        //Assert.Equal(result.Value, expectedItem.AsDto());

    }

    [Fact]
    public async void GetItems_ExisitngItems_ReturnsAllItems()
    {
        var expectedItems = new[]
        {
            CreateRandomItem(),
            CreateRandomItem(),
            CreateRandomItem(),
        };
        repositoryMock.Setup(repo => repo.GetItemsAsync())
        .ReturnsAsync(expectedItems);
        var controller = new ItemsController(repositoryMock.Object, loggerMock.Object);

        var res = await controller.GetItems();

        res.Value.Should().BeEquivalentTo(expectedItems);
    }

    [Fact]
    public async void CreateItem_CreatesSuccesfully_ShouldReturn()
    {
        var itemToCreate = new CreateItemDto() { Name = "Knife", Price = 3334 };
        var controller = new ItemsController(repositoryMock.Object, loggerMock.Object);

        var res = await controller.CreateItem(itemToCreate);

        repositoryMock.Verify(repo => repo.CreateItemAsync(It.IsAny<Item>()), Times.Once());
        Assert.IsType<CreatedAtActionResult>(res.Result);
        var createdItem = (res.Result as CreatedAtActionResult).Value as ItemDto;

        itemToCreate.Should().BeEquivalentTo(createdItem,
           options => options.ComparingByMembers<ItemDto>().ExcludingMissingMembers());

        createdItem.Id.Should().NotBeEmpty();
        createdItem.CreatedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));

    }


}