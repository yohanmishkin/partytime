# partyTime [![Build Status](https://travis-ci.org/yohanmishkin/partytime.svg?branch=master)](https://travis-ci.org/yohanmishkin/partytime)

## Conventions
- Resources must have an Id
- Related resources without Id property will not be serialized
- Related resources are not included by default (must be declared in query params for inclusion)

### Ideas
- Cast everything as Entity()?