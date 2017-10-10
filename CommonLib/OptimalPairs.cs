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
        public List<Item> Compute(Item[] catalog, uint limit, uint friends = 2)
        {
            var result = new List<Item>();
            int r = catalog.Length - 1;
            int l = 0;

            //check for invalid entries
            if(
                friends > 0 //atleast one item need to be selected
                || catalog == null //no catalog
                || catalog.Length <= 0 //empty catalog
                || catalog.Length < friends // items in catalog are less than friends, DISTINCT not possible
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
            for (int i = 0; i < friends - 1; i++)
            {
                lsum += catalog[i].Price;
            }
            uint totalSum = catalog[r].Price + lsum;
            //Slide r leftward until totalsum <= limit
            //O(n)
            while (totalSum > limit && r >= l)
            {
                r--;
                //if no. of valid items is less than number of friends, then we cant get any combination
                if (r - l + 1 < friends)
                {
                    return result;
                }
                totalSum = catalog[r].Price + lsum;
            }

            if (friends == 1) //only one item is requested
            {
                //here Friends = 1, return the lth or rth item
                result.Add(catalog[ r< l? l: r]);
                return result;
            }

            //HERE we have at least one valid combination of items and Friends >= 2
            //[l, l+1, l+2 .... l + Friends -2, r]
            //the strategy here is for every combination of l, r 
            // iterate through l+1.. r+1 for Friends - 2 times.
            // if Price[l] + Price [r] > limit then r-- else r++ and keep track of max sum in each iteration
            
            //Runtime if Friends = 2 then this can be achieved in O(n), else its factorial O(n * C(n, Friends-1))
            int prevL = l;
            int prevR = r;
            int remaining = (int)friends - 2;

            int[] prevOthers = new int[remaining];

            if(friends - 2 > 0)
            {
                for(int i = 0; i < remaining; i++)
                {
                    prevOthers[i] = l + 1 + i;
                }
            }

            if (GetTotalPrice(catalog, l, r, prevOthers) != limit)
            {
                int maxPrice = Int32.MinValue;
                while (r - l + 1 >= friends)
                {
                    if (friends == 2)
                    {
                        var totalPrice = GetTotalPrice(catalog, l, r, prevOthers);
                        if (totalPrice == limit)
                        {
                            prevL = l;
                            prevR = r;
                            break;
                        }
                        if (totalPrice > maxPrice && totalPrice < limit)
                        {
                            prevL = l;
                            prevR = r;
                            continue;
                        }
                        if (totalPrice < limit) l++;
                        else r--;
                    }
                    else
                    {
                        bool perfectFound = false;
                        bool moveForward = false;
                        bool firstComboSet = false;

                        foreach (var combination in GetNextCombination(l, r, (int)friends - 2))
                        {
                            var totalPrice = GetTotalPrice(catalog, l, r, combination);
                            if(!firstComboSet)
                            {
                                //set direction for either l++ or r--
                                firstComboSet = true;
                                moveForward = totalPrice < limit;
                            }
                            if (totalPrice == limit)
                            {
                                prevL = l;
                                prevR = r;
                                prevOthers = combination;
                                perfectFound = true;
                                break;
                            }
                            if (totalPrice > maxPrice && totalPrice < limit)
                            {
                                prevL = l;
                                prevR = r;
                                prevOthers = combination;
                                continue;
                            }
                        }
                        if (perfectFound)
                        {
                            break;//we dont care about other combinations 
                        }
                        if (moveForward) l++;
                        else r--;
                    }
                }
            }

            result.Add(catalog[prevL]);
            if (friends - 2 > 0)
            {
                for (int i = 0; i < remaining; i++)
                {
                    result.Add(catalog[prevOthers[i]]);
                }
            }
            result.Add(catalog[prevL]);
            return result;
        }

        uint GetTotalPrice(Item[] catalog, int l, int r, int[] others)
        {
            uint result = catalog[l].Price + catalog[r].Price;
            for(int i=0; i< others.Length; i++)
            {
                result += catalog[others[i]].Price;
            }
            return result;
        }

        //Get all array combination of indexes between a[l+1]..a[r-1] total of count items
        private IEnumerable<int[]> GetNextCombination(int l, int r, int count)
        {
            if (count <= 0) yield break;

            int[] a = new int[r - l - 1];
            for(int i = 0; i < r - l - 1; i++)
            {
                a[i] = l + 1 + i;
            }
            foreach(var combination in GetCombinations(a, 0, count))
            {
                yield return combination.ToArray();
            }
        }

        //recursively get combinations of array, starting from s of k items
        //inspired from https://pastebin.com/qpzvMX8L
        private List<List<int>> GetCombinations(int[] array, int s = 0, int k = 2)
        {
            var result = new List<List<int>>();
            if (k == 2)
            {
                var index = 0;
                for (int i = s; i < array.Length; i++)
                {
                    for (int j = i + 1; j < array.Length; j++)
                    {
                        result.Add(new List<int>());
                        result[index].Add(array[i]);
                        while (result[index].Count < k)
                        {
                            result[index].Add(array[j]);
                        }
                        index++;
                    }
                }
                return result;
            }

            var moreSubsets = new List<List<int>>();
            for (int i = s; i < array.Length - k + 1; i++)
            {
                result = GetCombinations(array, i + 1, k - 1);
                for (int index = 0; index < result.Count; index++)
                {
                    result[index].Insert(0, array[i]);
                }

                for (int y = 0; y < result.Count; y++)
                {
                    moreSubsets.Add(result[y]);
                }
            }
            return moreSubsets;
        }
    }
}
