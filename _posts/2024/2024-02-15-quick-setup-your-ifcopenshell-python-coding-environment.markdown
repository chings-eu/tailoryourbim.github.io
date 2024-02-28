---
layout: post
title: "Quick Setup Your IfcOpenShell Python Coding Environment"
date: 2024-02-15-214352 
categories: IfcOpenShell
tags: Python IfcOpenShell Ifc
published: true
---
<script src="/assets/signup/signup.js"></script>
<signup-component></signup-component>

# Quick virtual environment setups
First of all, install Python and Visual Studio Code


## A folder as environment
```terminal
mkdir demos
cd demos
code .
```
# **Python** and **IfcOpenShell**

```fsharp
// F# code
let a = 20
```

{% highlight terminal%}
pip install ifcopenshell
{% endhighlight%}

{% highlight python%}
# Python code
import pandas as pd
import ifcopenshell
pth = r""
ifc = ifcopenshell.open(pth)
print(ifc)
{% endhighlight%}

```python
print("Hello everyone!")
```

# Setup **Visual Studio Code**

# Setup Python **Virtual Environment**

# Install **Packages**

# **Workflow** Example

## Issues
### Pset in model not fitting requirement from the platform
Goal: Linking element to create cashflow
Process: See the needs of positions, edit geometry accordingly, gather properties and correct quantities
Contract posisitions, Schedule positions
Deal with:
    Geometry, properties, quantities
    Pset, Qset
    Assembly, Curtain walls, Stairs

### Where are the elements with changed identities
Goal: Find if there are elements with different ifc identity
Process: Find and mark the global ids not existing in both models
Contract positions
Deal with:
    Properties, geometry
    Changed element identities from two models
    Setup a customized property set


### How to compare the changing element properties from three models
Goal: Listing the property difference for the same global id
Process: Ask wanted information and list in Excel sheets
Deal with:
    Properties
    Three different model versions
    Reading properties, writing to Excel


## What we have learned?
Quick setup Python virtual environment
Quick understanding Ifc model structure - dig in relations (property set, assembly) - Python, Jupyter, Pandas
Property set manipulation - IfcOpenShell
Retrieving Base Quantities - SimpleBim, IfcOpenShell
Spatial containment modification - IfcOpenShell API
Model geometry comparison - overlapping geometry, different global id - IfcOpenShell, Solibri
Multiple model historical comparison - same global id, different properties - IfcOpenShell, Excel

