# jloxcs

A Csharp port of the Lox tree-waliking interpreter from Bob Nystrom’s [*Crafting Interpreters*](https://craftinginterpreters.com/) book. An attempt is made to stay close to the original Java code so one easily can refer to the Crafting Interpreters book for explanations. Nonetheless, to get jlox working in Csharp some differences are unavoidable.

## Noted differences from jlox (or mostly Java vs C#)
-   "clock()" is defined in NativeFunctions.cs. There were problems trying to cram the clock class into to the function argument as jlox did.
-   Csharp dictionary behave differently from Java hashmap.
-   The Resolver's "scopes "stack was implemented as a list. C# doesn't access stacks by index number (in resolveLocal). There were work arounds to use Linq, but I prefer doing things in a minimal way.
-   The TokenType enum members maintain their prefix.
-   Subtring behaves differently in C#. In Java, the second argument is the ending index. In C#, the second argument is the length of the substring to be retrieved.
-   Public access modifier is placed where Java has none. It seems the implied default Java modifier is analogous to C#' "internal".
-   The override modifier is not necessary for C# interfaces (and in fact, shows up as an error).
-   "toString" in jlox need not be an override in Csharp (due to capitalization differences).
-   globals cannot be assigned to environment in Csharp's field initializations, hence it is done on Interpreter's constructor. An alternative coule be to make globals static.
-   object is used instead where jlox uses "Void"
-   ParseError is not allowed to be static.

That being said, despite not coding more than 100 lines of Java many many years ago (Hello World was enough to make me hate it back then), it is fairly safe to say that Java and C# are very similar. 
