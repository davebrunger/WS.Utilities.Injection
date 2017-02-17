# WS.Utilities.Injection
Basic utilities for performing dependency injection tasks in .Net Core.

BasicInjectionContainer is, as the name says, a basic injection container.

 1. Types and instances can be registered with the container and the resolved at a later date. 
 2. BasicInjectionContainer supports only constructor injection. 
 3. When registering a resolving type it must have a single constructor that either takes no parameters, or only parameters that are registered with the container. 
 4. Dependencies are resolved at resolve time so they can be registered in any order. 
 5. Once the container has instantiated an instance of a resolving type, the same instance will be return each time it is requested.
 6. No work has yet been done to ensure that BasicInjectionContainer thread safe.

## Usage

### Creating the Container

    var container = new BasicInjectionContainer();

### Registering Types and Instances

Register an instance to the container that has already been created:

    var logger = new Logger();
    container.RegisterInstance(logger);

Register an instance to the container that implements an interface, or is the sub-type of some super type:

	var messageService = new ConsoleMessageService();
	container.RegisterInstance<IMessageService>(messageService);

Register a type to the container:

    container.RegisterType<MathsService>();

Register a type to the container that should be resolved to an implementing or inherited type. Useful if the consumers require interfaces in their constructors:

    container.RegisterType<IAddService, MathsService>();

### Resolving Types

Get an instance from the container

    var mathsService = container.Resolve<MathsService>();
    var sum = mathsService.Add(1, 2);