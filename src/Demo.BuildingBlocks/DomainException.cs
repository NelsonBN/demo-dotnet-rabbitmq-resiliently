using System;

namespace Demo.BuildingBlocks;
public class DomainException : Exception
{
    public DomainException(string reason) : base(reason) { }
}
