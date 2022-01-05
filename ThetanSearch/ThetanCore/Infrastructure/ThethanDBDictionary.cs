using LiteDB;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Collections;

namespace ThetanCore
{
  public class ThethanDBDictionary : IDictionary<string, Thetan>
  {
    private readonly IThetanRepository thetanRepository;

    public ThethanDBDictionary(IThetanRepository thetanRepository) : base()
    {
      this.thetanRepository = thetanRepository;
    }

    public Thetan this[string key]
    {
      get => this.thetanRepository.Select(key);
      set => this.thetanRepository.Upsert(value);
    }

    public ICollection<string> Keys => this.thetanRepository.Select(x => x.Id);

    public ICollection<Thetan> Values => this.thetanRepository.SelectAll();

    public int Count => this.thetanRepository.SelectAllCount();

    public bool IsReadOnly => false;

    public void Add(string key, Thetan value)
    {
      this.thetanRepository.Upsert(value);
    }

    public void Add(KeyValuePair<string, Thetan> item)
    {
      this.thetanRepository.Upsert(item.Value);
    }

    public void Clear()
    {
      this.thetanRepository.DeleteAll();
    }

    public bool Contains(KeyValuePair<string, Thetan> item)
    {
      return this.thetanRepository.Select(x => x.Id == item.Key).Any();
    }

    public bool ContainsKey(string key)
    {
      return this.thetanRepository.Select(x => x.Id == key).Any();
    }

    public void CopyTo(KeyValuePair<string, Thetan>[] array, int arrayIndex)
    {
      throw new NotImplementedException();
    }

    public IEnumerator<KeyValuePair<string, Thetan>> GetEnumerator()
    {
      return this.thetanRepository.SelectAll()
          .Select(x => new KeyValuePair<string, Thetan>(x.Id, x))
          .ToList().GetEnumerator();
    }

    public bool Remove(string key)
    {
      return this.thetanRepository.Delete(x => x.Id == key) == 1;
    }

    public bool Remove(KeyValuePair<string, Thetan> item)
    {
      return this.thetanRepository.Delete(x => x.Id == item.Key) == 1;
    }

    public bool TryGetValue(string key, out Thetan value)
    {
      value = null;
      try
      {
        value = this.thetanRepository.SelectWhere(x => x.Id == key).FirstOrDefault();
        return true;
      }
      catch
      {
        return false;
      }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return this.thetanRepository.SelectAll().ToList().GetEnumerator();
    }

  }
}

