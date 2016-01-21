#Why NoSQL-database?
There is currently a lot of technical interest in "Big Data". Extreme examples are: data collection and analyses from the Large Hadron Collider, the Sloan Sky Survey, analyses of Biological Genomes, collecting data for global climate models, and analyzing client interactions in social networks.

Conventional SQL databases may not be well suited for these kinds of applications. While they have worked very well for many business applications and record keeping, they get overwhelmed by massive streams of data. Developers are turning to "noSQL" databases like MongoDB, CouchDB, and Redis to handle massive data collection and analyses.

#NoSQL data model:
The data models used by noSQL databases are usually based on key/value pairs. noSQL processing favors modeling flexibility, the ability to easily scale out across multiple machines, and performance with very large datasets. For that flexibility they give up real-time data consistency, accepting application enforced eventual consistency. They give up a formal query mechanism (hence the name). And, they may give up Durability guarantees by only occasionally writing to persistant storage in order to provide high throughput with large volumes of data.

#Project-2
In this project, implemented a Key/Value based in-memory noSQL database in C# using .NET Framework Class Library and Visual Studio 2015

Build process:

devenv ProjectTwo.sln /Rebuild debug

Run from Developer Command Prompt for Visual Studio
