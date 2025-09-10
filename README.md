<div align="center">
    <img src="Documentation~/Images/Banner.png" alt="Package Banner">
</div>

## Table of Contents

* [Gameplay Tags for Unity](#gameplay-tags-for-unity)

  * [Overview](#overview)
  * [Features](#features)
  * [Installation](#installation)
  * [Usage](#usage)

    * [Registering Tags](#registering-tags)

      * [Assembly Attributes](#1-assembly-attributes)
      * [JSON Files in Project Settings](#2-json-files-in-project-settings)
    * [GameplayTagCountContainer](#gameplaytagcountcontainer)

      * [Creating a Tag Container](#creating-a-tag-container)
      * [Adding a Tag](#adding-a-tag)
      * [Removing a Tag](#removing-a-tag)
      * [Registering a Callback for Tag Changes](#registering-a-callback-for-tag-changes)
      * [Removing a Callback](#removing-a-callback)
      * [Querying the Count of a Tag](#querying-the-count-of-a-tag)
      * [Clearing All Tags](#clearing-all-tags)
    * [GameplayTagContainer](#gameplaytagcontainer)

      * [Creating a Tag Container](#creating-a-tag-container-1)
      * [Adding a Tag](#adding-a-tag-1)
      * [Removing a Tag](#removing-a-tag-1)
      * [Clearing All Tags](#clearing-all-tags-1)
    * [Union and Intersection Operations](#union-and-intersection-operations)

      * [Creating a Union of Tag Containers](#creating-a-union-of-tag-containers)
      * [Creating an Intersection of Tag Containers](#creating-an-intersection-of-tag-containers)
  * [Differences between GameplayTagCountContainer and GameplayTagContainer](#differences-between-gameplaytagcountcontainer-and-gameplaytagcontainer)
  * [AllGameplayTags Generated Class](#allgameplaytags-generated-class)
  * [License](#license)

## Overview

This project is an implementation of gameplay tags, similar to those found in Unreal Engine, for use in Unity. Gameplay tags are a flexible and efficient way to handle and categorize gameplay-related properties and states.

## Features

- Tag-based system for categorizing and managing gameplay elements.
- Easy integration with existing Unity projects.
- Flexible tagging system to suit a wide variety of use cases.

## Installation

1. Clone the repository or download the latest release.
2. Open your Unity project.
3. Add the package to your project using the Unity Package Manager:
   - Click on `Window -> Package Manager`.
   - Click the `+` button and select `Add package from git URL...`.
   - Enter the following URL:
     ```
     https://github.com/BandoWare/GameplayTags.git
     ```
   - Click `Add`.

## Usage

### Registering Tags

Gameplay tags can now be registered in **two different ways**:

#### 1. Assembly Attributes

Use this approach when your code requires certain tags to exist. For example:

```csharp
[assembly: GameplayTag("Damage.Fatal")]
[assembly: GameplayTag("Damage.Miss")]
[assembly:GameplayTag("CrowdControl.Stunned")]
[assembly:GameplayTag("CrowdControl.Slow")]
```

#### 2. JSON Files in Project Settings

Alternatively, you can register tags through **JSON files**.
Create a folder named `ProjectSettings/GameplayTags` in your Unity project.
Every `.json` file inside this folder will be scanned and its tags automatically registered.

The JSON format is based on an object where each property is a tag name.

* The value of the property must be another object.
* You can leave it empty, or add metadata:

  * `Comment`: developer-facing description.
  * `Children`: nested object containing child tags.

👉 **Important notes:**

* You can declare tags **directly** at the root level, or organize them with `Children`.
* Both approaches can be **mixed in the same file**.
* The `Children` property is **recursive**, so tags can be nested at any depth if needed.

---

#### Example mixing both styles

```json
{
  // Direct tags
  "CrowdControl.Standard": {},
  "Damage.Fatal": {},

  // Direct tag with a comment
  "Damage.Miss": {
    "Comment": "Attack landed but did not cause damage"
  },

  // Hierarchical tags
  "CrowdControl": {
    "Comment": "Crowd control tags",
    "Children": {
      "Stunned": {
        "Comment": "Unit cannot act at all"
      },
      "Slow": {}
    }
  }
}
```

### GameplayTagCountContainer

`GameplayTagCountContainer` is a class used to manage gameplay tags with event callbacks for tag count changes. Here’s how to use it:

#### Creating a Tag Container

```csharp
GameplayTagCountContainer tagContainer = new GameplayTagCountContainer();
```

#### Adding a Tag

```csharp
GameplayTag tag = GameplayTagManager.RequestTag("ExampleTag");
tagContainer.AddTag(tag);
```

#### Removing a Tag

```csharp
tagContainer.RemoveTag(tag);
```

#### Registering a Callback for Tag Changes

```csharp
void OnTagChanged(GameplayTag tag, int newCount)
{
    Debug.Log($"Tag {tag.Name} count changed to {newCount}");
}

tagContainer.RegisterTagEventCallback(tag, GameplayTagEventType.AnyCountChange, OnTagChanged);
```

#### Removing a Callback

```csharp
tagContainer.RemoveTagEventCallback(tag, GameplayTagEventType.AnyCountChange, OnTagChanged);
```

#### Querying the Count of a Tag

```csharp
int tagCount = tagContainer.GetTagCount(tag);
Debug.Log($"Tag {tag.Name} has a count of {tagCount}");
```

#### Clearing All Tags

```csharp
tagContainer.Clear();
```

### GameplayTagContainer

`GameplayTagContainer` is a class for storing a collection of gameplay tags. It is serializable and provides a user-friendly interface in the Unity editor.

#### Creating a Tag Container

```csharp
GameplayTagContainer tagContainer = new GameplayTagContainer();
```

#### Adding a Tag

```csharp
GameplayTag tag = GameplayTagManager.RequestTag("ExampleTag");
tagContainer.AddTag(tag);
// or 
tagContaier.AddTag("ExampleTag");
```

#### Removing a Tag

```csharp
tagContainer.RemoveTag(tag);
```

#### Clearing All Tags

```csharp
tagContainer.Clear();
```

### Union and Intersection Operations

Union and intersection operations can be performed on any type of container that implements `IGameplayTagContainer`. These operations can be used to create new `GameplayTagContainer` instances.

#### Creating a Union of Tag Containers

```csharp
GameplayTagContainer union = GameplayTagContainer.Union(container1, container2);
```

#### Creating an Intersection of Tag Containers

```csharp
GameplayTagContainer intersection = GameplayTagContainer.Intersection(container1, container2);
```

## Differences between GameplayTagCountContainer and GameplayTagContainer

- **GameplayTagCountContainer**: Focuses on managing tags with the ability to register callbacks for when tag counts change. It is useful when you need to respond to tag count changes.
- **GameplayTagContainer**: Designed to store a collection of tags, it is serializable and offers a user-friendly interface in the Unity editor. It provides basic tag management without the event-driven functionality of `GameplayTagCountContainer`.

## AllGameplayTags Generated Class

A Source Generator provides access to any gameplay tag declared within the current assembly without requiring a dedicated field to store the tag value. This approach eliminates the need to repeatedly call `GameplayTagManager.RequestTag`. For example, the gameplay tag "A.B.C" can be accessed through `AllGameplayTags.A.B.C.Get()`, simplifying tag retrieval and enhancing performance by avoiding redundant tag requests. [Read More](Documentation~/CodeGeneration.md).

## License

This project is licensed under the Creative Commons Attribution 4.0 International (CC BY 4.0) License. See the [LICENSE](LICENSE.md) file for details.