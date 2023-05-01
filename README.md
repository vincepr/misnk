# Learning how to write a compiler

Writing a Lexer - Parser - Interpreter should provide me a better grasp about usage of Trees and how to manage a growing set of functionalty. Coding in a way to keep tings extendable etc.

C# is currently subject in my education and the video-guide by [Immo Landwerth](https://www.youtube.com/@ImmoLandwerth) sounds like a good side project to follow along myself

- The video guide: https://github.com/terrajobst/minsk/tree/master/docs
- Another great Ressource. An awesome free book about writing a compiler/interpreter and it's inner workings: https://craftinginterpreters.com/

# Random Notes
##  Syntax Tree, AST
```
    // 1 + 2 * 3
    // gets parsed into a treel like:
    //
    //    +
    //   / \
    //  1   *
    //     / \
    //    2   3
```
- Contains all the information the Parser needs to walk it to produce the code/output
- The parse Tree is a concrete representation of the input. So it includes whitespace, end of lines etc... (one could recreate the document with it)
- The AST on the other hand is an abstract representation of the input. No whitespaces, comments etc. But also Other Info like enclosing Brackets (). Since Info about those gets represented by how the tree itself is structured.

## To Add Type checking
Episode 2 - 38:00 and onward.

The idea is to create an aditional Tree (Bound Tree) to store information about what types expressions reslove into etc.
