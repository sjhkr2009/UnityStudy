using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerStatModifier {
    float ApplyModify(CharacterData characterData);
}
