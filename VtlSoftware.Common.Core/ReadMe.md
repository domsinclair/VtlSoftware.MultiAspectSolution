# VtlSoftware Common Core

A collection of code, based on Net 6, that is intended for reuse in Aspects built with Metalama.

## Contents

### Dependencies

This library takes a Dependency off Microsoft>Extensions.Logging.Console. By taking this down to console level it should be possible to send log messages out to the console without any need to reference A Microsoft.Extensions.Logging component in the future.

### InterpolatedStringHandler 

Code that implements an InterpolatedStringHandler.  This code is fundamental to allowing Logging Aspects to produce log messages that can be used in both text based logging frameworks and Structered logging frameworks.

### Log Recursion Guard

As its name suggests a guard whose job it is to prevent possible recursive llos in logged code from taking place.
