1. I would suggest blazor because it is new? not really because on how it interract between pages it is like webform that stateful can pass event 
through pages and lot of awesome , its like par with some spa js framework like angular,react and vue 
2. About architecture we should try to use if posible the clean architecture, domain, application, core.
3. also CQRS since payroll is somehow process hungry system we just call events just not to block thread.
4. also event driven archi not really used to but I think it is good for this type of system
5. about dtos and validation we should try to look other like automapper, fluent validation its a cool stuff
6. about UI not really a UI dev but I would suggest admin lte for the win because of side nav hehe

the most I struggle is the way that the project is built I didnt use business as project instead of application, so I add another project that would store all my services,
and also domain project which is contain most of my data access and model,
also the manual dto I seldom used does and separate dto in each action not used to it.
and also didnt use factory pattern before maybe I used it and I heard it but I really not using it I always used the services to transform everything;
