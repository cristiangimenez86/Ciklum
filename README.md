# Notes

### Storage
I'm using Entity Framework in-memory database

### Seeds
Assumption: I'm populating [Team] entity with a list of FIFA country codes since it is a football World Cup. </br>
Source: https://en.wikipedia.org/wiki/List_of_FIFA_country_codes 

### Dependency Injection
* IServiceCollection Extension method: **AddScoreBoardService()**
* Manual declaration of:
  - IScoreBoardService
  - IValidator
  - IGameRepository
  - Database Context: ScoreBoardDbContext

### Testing
* Unit Tests: **UnitTests.ScoreBoardServiceTests.cs**
* Integration Tests: **IntegrationTests.ScoreBoardServiceTests.cs**

### Commit history
https://github.com/cristiangimenez86/Ciklum/commits/main/
