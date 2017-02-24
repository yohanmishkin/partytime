# partyTime [![Build status](https://ci.appveyor.com/api/projects/status/d3excs9s82x75vjs?svg=true)](https://ci.appveyor.com/project/yohanmishkin/partytime)

## Conventions
- Resources must have an Id
- Related resources without Id property will not be serialized
- Related resources are not included by default (must be declared in query params for inclusion)

### Ideas
- Cast everything as Entity()?