# What is it?
This is perhaps the most simple math expression lexer and parser written on clean C#, so I guess it helps someone to understand a conception of parsers developing. See: [OopExpressionParser/Parser](https://github.com/rodion-m/OopExpressionParser/tree/master/OopExpressionParser/Parser)

It's able to evaluate simple expressions like `"2+2*2"` (returns 6). It supports `+, -, *, /` operations. But you can easyly add your own operation just by addig a new class inherited from `Operation` class and it became work. So it's super easy.
Brackets are not supported at the moment.
