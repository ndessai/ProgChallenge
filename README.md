# Programming Challenges

The solution has been hosted on http://progchallenge.azurewebsites.net and it can be tested as follows.
# 1. For hash testing
curl -X POST -H "Content-Type: application/json" -d '{"message": "foo"}' http://progchallenge.azurewebsites.net/api/messages
curl 
http://progchallenge.azurewebsites.net/api/messages/2c26b46b68ffc68ff99b453c1d30413413422d706483bfa0f9
8a5e886266e7ae
# 2. For Replace char testing 
curl http://progchallenge.azurewebsites.net/api/replace/X01X


This solution is built in Visual Studio (C#) and the projects are organized as follows.

# 1. Server
  This is the REST api backend to support hashing the input string. The uploaded messages are hashed. The digest and the message is persisted in sql db. 
  The GET api will retrieve the message and the digest from sql db and is returned to the caller.
  The POST api computes the digest and add it to the sql db.
  
# 2. CommonLib 
  This project houses both the algorithmic challenges. Only the core logic is added to this common lib and the driver applications are     built separately. This is also used in the server, so the algorithmic questions can be tested directly through REST API and so the project does not need to be built locally or run locally.
# 2. a. ReplaceChar
  Runtime -> O(n * 2^x) - where n is the length of the string and x is number of x's in the input string. 
  The code iterates through n chracters of the string 1 by 1 and as it encounters the x it duplicates the existing prefixes.
      
# 2. b. OptimalPairs
   This has been generalized to allow getting n pairs for n friends. The original question is for n=2 friends and bonus n=3 friends. However, I have made an attempt to include this for as many friends as possible for items in the prices.txt. 
      
For 2 friends, the pair can be optimally computed in O(n) by sliding left and right of the array appropriately.
However, for more friends, while left, right indices can be computed in O(n), I think the other indices will grow factorially resulting in O(n * C(n, Friends-1)) complexity. Here C(n, k) is combinatorics for getting k items from n.
      

