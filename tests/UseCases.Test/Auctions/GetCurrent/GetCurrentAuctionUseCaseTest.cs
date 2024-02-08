using Bogus;
using FluentAssertions;
using Moq;
using RocketseatAuction.API.Contracts;
using RocketseatAuction.API.Entities;
using RocketseatAuction.API.Enums;
using RocketseatAuction.API.UseCases.Auctions.GetCurrent;
using Xunit;

namespace UseCases.Test.Auctions.GetCurrent;
public class GetCurrentAuctionUseCaseTest
{
    [Fact]
    public void Success()
    {
        //ARRANGE
        var mock_auction = new Faker<Auction>()
                            .RuleFor(auction => auction.Id, f => f.Random.Number(1, 100))
                            .RuleFor(auction => auction.Name, f => f.Lorem.Word())
                            .RuleFor(auction => auction.Starts, f => f.Date.Past())
                            .RuleFor(auction => auction.Ends, f => f.Date.Past())
                            .RuleFor(auction => auction.Items, (f, auction) => new List<Item>
                            {
                                new Item
                                {
                                    Id = f.Random.Number(1,100),
                                    Name = f.Commerce.ProductName(),
                                    Brand = f.Commerce.Department(),
                                    BasePrice = f.Random.Decimal(50, 1000),
                                    Condition = f.PickRandom<Condition>(),
                                    AuctionId = auction.Id
                                }
                            }).Generate();

        var auctionRepository = new Mock<IAuctionRepository>();
        auctionRepository.Setup(i => i.GetCurrent()).Returns(mock_auction);

        var useCase = new GetCurrentAuctionUseCase(auctionRepository.Object);

        //ACT
        var auction = useCase.Execute();

        //ASSERT
        auction.Should().NotBeNull();
        auction.Id.Should().Be(mock_auction.Id);
        auction.Name.Should().Be(mock_auction.Name);
    }
}
