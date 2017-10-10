# Programming Challenges

The solution is hosted on http://progchallenge.azurewebsites.net. The source is built on asp.net core (since it is cross platform) with Visual Studio. The programmmig challenges can be built as console apps and tested. However, to avoid steps to build it locally and run, I have moved the core logic to a shared library and have frontended these with REST apis for easy testing just through browser.

Please follow steps below to test the functionality.

### Message and Digest
Use following command to request the message for the input digest

```
curl -i http://progchallenge.azurewebsites.net/api/messages/2c26b46b68ffc68ff99b453c1d30413413422d706483bfa0f98a5e886266e7ae
```
The very first request is expected to return 404 as specified. (You could change the digest in the request to simulate the 404).

![Alt text](/Images/Digest404.PNG?raw=true "404 output as digest not found")

You can then post a request for generating the digest using
```
curl -X POST -H "Content-Type: application/json" -d '{"message": "foo"}' http://progchallenge.azurewebsites.net/api/messages
```

![Alt text](/Images/DigestGenerate.PNG?raw=true "Generate digest")

Now, if you resubmit the GET request as below, you will retrieve the message
```
curl -i http://progchallenge.azurewebsites.net/api/messages/2c26b46b68ffc68ff99b453c1d30413413422d706483bfa0f98a5e886266e7ae
```

![Alt text](/Images/DigestRetrieve.PNG?raw=true "Retrieve digest")

#### Notes: 
* The digest and messages are persisted in MS SQL db.
* For the sake of completeness, the service also supports GET to retrieve all digests and DELETE a digest.
* BONUS QUESTION: What would the bottlenecks in your implementation be as you acquire more users. How you might scale your microservice?
  The bottlenecks would be contention to the shared storage where the digests and messages are stored. In this case, its MS SQL server     and the lock on the db to store the digest. One of the ways to handle is partitioning/sharding. You could begin by directing the all     digests that start with say 'a' to be stored in a specific db. A different db for 'b' etc.
  
  For retrieval if security is not a concern, you could use ETag header in the GET response, so the edge devices will cache the message   for each corresponding hash and thus the GET requests would never reach the server. Since each message will generate unique hash, this   will naturally support the caching without requiring to expire the cache.


### Replace 
This can be tested by simply typing below address in the browser directly

```
http://progchallenge.azurewebsites.net/api/replace/X0110X0001
```
The last parameter here is the string. The browser would show all possible combinations where X is replaced with both 1 and 0.

![Alt text](/Images/ReplaceCharSuccess.PNG?raw=true "Replace Xs")

If the input string contains some other character (for completeness, I have allowed both X and x), it will error as below.

![Alt text](/Images/ReplaceCharFailure.PNG?raw=true "Replace Xs with invalid input")

#### Notes:
* Runtime -> O(n * 2^x) - where n is the length of the string and x is number of x's in the input string. 
  The code iterates through n chracters of the string one by one and as it encounters the x it duplicates the existing prefixes.


### Gift card optimization
For simplicity of testing, I have hosted a very simple/bare bone html at 

```
http://progchallenge.azurewebsites.net/index.html
Click "Choose File". If the file is well formatted as specified, it should show the contents 
Set the gift card limit. 3000 default.
Set the number of Friends: 2 default. This is number of items you want to be picked up.
Click "Find Pairs".
```
![Alt text](/Images/FindPair.PNG?raw=true "Find pairs to optimize gift card usage")

When no combination is possible, you should expect
![Alt text](/Images/FindPairFailure.PNG?raw=true "No Combinations possible")

#### Notes:
* The bonus question was for 3 friends. However, once the number is 3 or higher, it has similar logic and hence I generalized it to any   number of friends.
* For 2 friends, the pair can be optimally computed in O(n) by sliding left and right of the array appropriately.
  However, for more friends, while left, right indices can be computed in O(n), I think the other indices will grow factorially           resulting in O(n * C(n, Friends-1)) complexity. Here C(n, k) is combinatorics for getting k items from n.
      
#### About the solution organization
This solution is built in Visual Studio (C#) and the projects are organized as follows.

##### 1. Server
  This is the REST api backend to support hashing the input string, replacing characters and finding optimal pairs. There is some boiler   plate code generated through templates, but most of the code is to serialize/deserialize input/output and forward request to             CommonLib.
  
##### 2. CommonLib 
  This project houses both the algorithmic challenges. Only the core logic is added to this common lib and the driver applications are     built separately. This is also used in the server, so the algorithmic questions can be tested directly through REST API and so the       project does not need to be built locally or run locally.
##### 2. a. ReplaceChar
  Class with replace logic.
##### 2. b. OptimalPairs
  Class with Optimal Pairs logic.
##### 3. ReplaceX 
  Console app / driver for ReplaceX logic to be executed on command line.
##### 4. FindPair
  Console app / driver for FindPair logic to be executed on command line.

