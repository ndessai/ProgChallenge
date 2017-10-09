using System;
using System.Collections.Generic;

namespace CommonLib
{
    public class Item
    {
        public string Name { get; set; }
        public uint Price { get; set; }
    }

    public class OptimalPairs
    {
        public List<Item> Compute(Item[] Catalog, uint Limit, uint Friends = 2)
        {
            var result = new List<Item>();
            int r = Catalog.Length - 1;
            int l = 0;

            //check for invalid entries
            if(
                Friends > 0 //atleast one item need to be selected
                || Catalog == null //no catalog
                || Catalog.Length <= 0 //empty catalog
                || Catalog.Length < Friends // items in catalog are less than friends, DISTINCT not possible
                )
            {
                return result; //ideally throw an exception, but that can be handled by calling function
            }

            //STEP 1: 
            //Possible combinations exist between l and r inclusive
            //let lsum = sum of leftmost Friends - 1 item prices
            //let totalsum = lsum + Catalog[r].Price
            //while(totalsum > limit) decrement r and recompute totalsum
            //O(n)
            uint lsum = 0;
            for (int i = 0; i < Friends - 1; i++)
            {
                lsum += Catalog[i].Price;
            }
            uint totalSum = Catalog[r].Price + lsum;
            //Slide r leftward until totalsum <= limit
            //O(n)
            while (totalSum > Limit && r >= l)
            {
                r--;
                //if no. of valid items is less than number of friends, then we cant get any combination
                if (r - l + 1 < Friends)
                {
                    return result;
                }
                totalSum = Catalog[r].Price + lsum;
            }

            if (Friends == 1) //only one item is requested
            {
                //here Friends = 1, return the lth or rth item
                result.Add(Catalog[ r< l? l: r]);
                return result;
            }

            //HERE we have at least one valid combination of items and Friends >= 2
            //[l, l+1, l+2 .... l + Friends -2, r]

            //
            //here we have valid "potential" combinations between l and r inclusive. The way we will go forward is
            //select lsum = sum of left most Friends-1 items.
            //keep on sliding "r" from right to left until lsum + items[r].Price <= limit
            //if such a combination exist then we found ATLEAST one valid combination.
            //O(n)

            //Step 1. Exclude all items whose price is higher than or Equal to Limit
            //O(n)
            while (Catalog[r].Price >= Limit)
            {
                r--;
            }

            

            //here we have valid "potential" combinations between l and r inclusive. The way we will go forward is
            //select lsum = sum of left most Friends-1 items.
            //keep on sliding "r" from right to left until lsum + items[r].Price <= limit
            //if such a combination exist then we found ATLEAST one valid combination.
            
            


            //Step 2. Compute difference array, this will help in deciding which window to slide, left or right. 
            //O(n)
            var diffArray = new uint[r + 1];
            diffArray[0] = 0;
            for(int i = 1; i < r; i++)
            {
                diffArray[i] = Catalog[i].Price - Catalog[i - 1].Price;
            }

            if(Catalog[l].Price + Catalog[r].Price > Limit || l >= r)
            {
                return result;//empty set
            }

            //NOTE. Do a binary search to find price = Limit/Friends. This is most optimal, if found
            //However, the specs look for DISTINCT items, so there is no point in doing this. 

            //Step 2. Iterate over all other remaining items computing sum and comparing
            // with previous highest and the limit
            //O(n)
            int minDiff = Int32.MaxValue;
            int lastValidLeft = l;
            int lastValidRight = r;
        
            while(l < r)
            {
                int currentDiff = (int) Math.Abs(Limit - (Catalog[l].Price + Catalog[r].Price));
                if (currentDiff < minDiff)
                {
                    minDiff = currentDiff;
                }
                else if(Catalog[l].Price + Catalog[r].Price < )
            }

            return result;
        }
    }
}
