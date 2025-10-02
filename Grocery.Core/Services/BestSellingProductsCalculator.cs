using Grocery.Core.Models;

namespace Grocery.Core.Services
{
    public class BestSellingProductsCalculator
    {
        public List<BestSellingProducts> Bereken(List<GroceryListItem> alleItems, List<Product> alleProducten, int topX)
        {
            var productVerkopen = alleItems
                .GroupBy(item => item.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    NrOfSells = g.Sum(x => x.Amount)
                })
                .OrderByDescending(x => x.NrOfSells)
                .Take(topX)
                .ToList();

            var productenDict = alleProducten.ToDictionary(p => p.Id);

            var resultaat = productVerkopen
                .Select((x, idx) =>
                {
                    var product = productenDict.GetValueOrDefault(x.ProductId);
                    return new BestSellingProducts(
                        x.ProductId,
                        product?.name ?? "Onbekend",
                        product?.Stock ?? 0,
                        x.NrOfSells,
                        idx + 1);
                })
                .ToList();

            return resultaat;
        }
    }
}