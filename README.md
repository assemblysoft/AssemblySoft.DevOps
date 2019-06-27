# AssemblySoft.DevOps
software DEVelopment and software OPerationS  library which includes a task runner capable of managing configurable light weight tasks 

### Goal
Simple DevOps library which enables independent development of tasks on prem


#### Why?
I had a set of tasks that that i wanted to run either sequentially or in parallel and didn't want the overhead of a full blown product. 
It covered two initial scenarios.

 - Build and Package some bespoke software that had a fairly complicated set of steps
 - Have a remote agent perform setup of developer machines off shore, including installing software, configuring repos and machine settings. https://github.com/assemblysoft/AssemblySoft.WonkaAgent
 
Since then, have utilised it to combine with on premise part of an overall Azure DevOps Pipeline. 



### Task
A task is the single unit that performs something meaningful. It could be to run a script, copy a directory, contact a web service or as simple as outputting a message to the console.

### Tasks Definition
A build, or execution of a set of steps, starts with a definition. A tasks definition is a collection of tasks that can run sequentially or in parallel.
A simple example of a set of tasks can be found here:
https://github.com/assemblysoft/AssemblySoft.DevOps/blob/master/AssemblySoft.DevOps.TestClient/data/build.tasks

The above example is presented as a serialised collection of DevOps tasks, serialized as xml. This could be any format but the example utilises an XML serializer. Once the tasks are loaded into memory, they can be processed.

#### DevOps Task
A DevOps task comprises of the following properties:

| Property        | Value           | Description  |
| ------------- |:-------------:| -----:|
| Id      | 79548566-e223-49ef-8296-d06a4a5dd48b | Unique id of the task (GUID) |
| Status      | Idle      |   Status of the task. (Get's updated during execution) |
| Assembly | AssemblySoft.DevOps.Task.Example.dll      |    .Net Assembly (Used to reference assembly via reflection) |
| Namespace | AssemblySoft.DevOps.Task.Example.DelayTask      |    .Net Assembly Namespace (Used to reference assembly via reflection) |
| Method | GoToSleepForTenSeconds      |    Name of the method (Needs to follow API convention) |
| Enabled | true      |    Whether task is currently enabled. (Disabled will ignore the task on next pass of the runner) |
| Order | N      |  The sequential order of processing. (Parallel execution can be enabled by using the same number)  |
| Description | Go to sleep for ten seconds      |  Meaningful description. (Will be used in output of the task)   |


### Coded Tasks
For more complex scenarios that require existing or new executable binaries to be run, custom build tasks can be created.
This enables proprietory tasks to live in your own source code repository and strung together as part of the definition via reflection. This makes it a simple process to construct a set of binaries and scripts as part of a managed workflow with very little effort. It also enables de-coupling of development as tasks can be developed separately, by different teams, with a degree of confidence as communication is handles by a simple Interface based API that all tasks adhere to.

### Task Runner
The task runner takes a tasks definition and executes each task either sequentially or in parallel.


### Usage
Included in the project is an example project and simple set of coded tasks that are defined in the task definition mentioned above.


### User Interface
The project has a console GUI but you can also make use of the richer web site:
https://github.com/assemblysoft/AssemblySoft.WonkaBuild

## Downloads
NuGet package via NuGet: AssemblySoft.DevOps

## License

AssemblySoft.DevOps is distributed under the MIT License.
