using System;
using System.Collections.Generic;
using System.Text;

namespace ThetanCore.Filters
{

  public interface IFilter<T>
  {
    /// <summary>
    /// Filter implementing this method would perform processing on the input type T
    /// </summary>
    /// <param name="input">The input to be executed by the filter</param>
    /// <returns></returns>
    T Execute(T input);
  }

  public abstract class Pipeline<T>
  {
    protected readonly List<IFilter<T>> filters = new List<IFilter<T>>();

    public Pipeline<T> Register(IFilter<T> filter)
    {
      filters.Add(filter);
      return this;
    }
    /// <returns></returns>
    public abstract T Process(T input);
  }

  /// <summary>
  /// Pipeline which to select final list of applicable agents
  /// </summary>
  public class ThethanSelectionPipeline : Pipeline<IEnumerable<Thetan>>
  {
    /// <summary>
    /// Method which executes the filter on a given Input
    /// </summary>
    /// <param name="input">Input on which filtering
    /// needs to happen as implementing in individual filters</param>
    /// <returns></returns>
    public override IEnumerable<Thetan> Process(IEnumerable<Thetan> input)
    {
      foreach (var filter in filters)
      {
        input = filter.Execute(input);
      }

      return input;
    }
  }

  //public class AgentAvailabilityFilter : IFilter<IEnumerable<Thetan>>
  //{
  //  public IEnumerable<Thetan> Execute(IEnumerable<Thetan> input)
  //  {
  //    if (input == null || input.Count() < 1)
  //    {
  //      return input;
  //    }
  //    return input.Where(thetan => thetan. == "Available");
  //  }
  //}
}
