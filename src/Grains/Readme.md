All Silos of the cluster should reference interface of all grain types of the cluser.
**Grain classes should only be referenced by the silos that will host them.**
**A given Grain Type implementation must be the same on each silo that supports it. Having several implmentations of the same grain interface is not valid!**