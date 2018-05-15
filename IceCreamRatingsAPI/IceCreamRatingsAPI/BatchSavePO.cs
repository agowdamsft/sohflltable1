using System;
using System.Collections.Generic;
using System.Text;

namespace IceCreamRatingsAPI
{
    public class BatchSavePO
    {

        public static async void SavePOAsync(PO po)
        {
            var dbRepo = new DocumentDBRepository<PO>("SOHFLLTable1", "PurchaseOrder");

            await dbRepo.CreateItemAsync(po);
        }

        public static bool ProcessPOInformation(string[] header, string[] orderItems, string[] products, string filename)
        {
            var result = false;

            var localPO = GetHeader(header, filename);

            localPO.LineItems = GetItems(orderItems, products);

            SavePOAsync(localPO);

            return result; 
        }

        private static LineItem[] GetItems(string[] orderItems, string[] products)
        {
            var result = new List<LineItem>();

            var isReady = false;

            foreach (var item in orderItems)
            {
                if (isReady)
                {
                    var itemsplit = item.Split(',');

                    var lineitem = new LineItem
                    {
                        PoNumber = itemsplit[0],
                         ProductId = itemsplit[1],
                        Quantity = long.Parse(itemsplit[2]),
                        Unitcost = double.Parse(itemsplit[3]),
                        Totalcost = double.Parse(itemsplit[4]),
                        Totaltax = double.Parse(itemsplit[5]),
                        ProductDescription = GetProductDescription(itemsplit[1], products),
                        ProductName = GetProductName(itemsplit[1], products)
                    };

                    result.Add(lineitem);
                }

                isReady = true;
            }

            return result.ToArray();
        }

        private static string GetProductName(string productid, string[] products)
        {
            foreach (var product in products)
            {
                
                var productsplit = product.Split(',');

                if (productid == productsplit[0])
                {
                    return productsplit[1];
                }

            }

            return string.Empty;
        }

        private static string GetProductDescription(string productid, string[] products)
        {
            foreach (var product in products)
            {
                var productsplit = product.Split(',');

                if (productid == productsplit[0])
                {
                    return productsplit[2];
                }

            }

            return string.Empty;
        }

        private static PO GetHeader(string[] header, string filename)
        {
            var headerFields = header[1].Split(',');
         
            return new PO
            {
                PoNumber = headerFields[0],
                FileName = filename,
                Datetime = headerFields[1],
                LocationId = headerFields[2],
                LocationName = headerFields[3],
                Locationaddress = headerFields[4],
                Locationpostcode = headerFields[5],
                Totalcost = double.Parse(headerFields[6]),
                Totaltax = double.Parse(headerFields[7])
            };
        }
    }
}
