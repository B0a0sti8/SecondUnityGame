using System.Collections.Generic;
using UnityEngine;

public class Stat
{
	public float baseValue=0;

	private List<float> modifiersAdd = new List<float>();
	private List<float> modifiersMultiply = new List<float>();

	public Stat()
    {
		baseValue = 0;
		modifiersAdd = new List<float>();
		modifiersMultiply = new List<float>();
	}


	public float GetValue()
	{
		float finalValue = baseValue;
		float additiveFactors = 1;

		modifiersAdd.ForEach(x => additiveFactors += x);
		finalValue *= additiveFactors;
		modifiersMultiply.ForEach(x => finalValue *= (1 + x));
		return finalValue;
	}

	public void AddModifierAdd(float modifier)
	{
		if (modifier != 0)
			modifiersAdd.Add(modifier);
	}

	public void RemoveModifierAdd(float modifier)
	{
		if (modifier != 0)
			modifiersAdd.Remove(modifier);
	}

	public void AddModifierMultiply(float modifier)
	{
		if (modifier != 0)
			modifiersMultiply.Add(modifier);
	}

	public void RemoveModifierMultiply(float modifier)
	{
		if (modifier != 0)
			modifiersMultiply.Remove(modifier);
	}
}
