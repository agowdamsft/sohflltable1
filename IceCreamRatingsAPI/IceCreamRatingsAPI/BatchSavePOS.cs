using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IceCreamRatingsAPI
{
    public class BatchSavePOS
    {


        public static async Task<bool> SavePOSAsync(POS pos)
        {
            var dbRepo = new DocumentDBRepository<POS>("SOHFLLTable1", "POS");

            await dbRepo.CreateItemAsync(pos);
            return true;
        }

    }



}
