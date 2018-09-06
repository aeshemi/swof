# swof

[![Build status](https://ci.appveyor.com/api/projects/status/gf74mahypkowfcs1?svg=true)](https://ci.appveyor.com/project/aeshemi/swof-5nu6s)

Selects two engineers at random to both complete a half day of support each (we assume to have 10 engineers).

#### Business Rules

* An engineer can do at most one half day shift in a day
* An engineer cannot have half day shifts on consecutive days
* Each engineer should have completed one whole day of support in any 2 week period

Schedule generation currently defaults to create rotations for 4 weeks from the last scheduled week. This is ideally triggered via a job in 2 week intervals so engineers could plan for their schedule (i.e swap schedules).
