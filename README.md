
Before starting set multiple projects as startup: MockBank and PaymentGateway
Right click the Solution > Properties > Common Properties > Startup project > Multiple Startup Projects

Optional implementations:
- Global exception handler
- Global Request/Response logger (should add extra logging to API calls)
- Swagger  Api documentation


Application Urls:
PaymentGateway http://localhost:57955/swagger/
MockBank http://localhost:58079/swagger/

Notes:
- Transaction Id generation is not ideal given the nature of GetHashCode(), that is, different objects may return the same Hash Code.