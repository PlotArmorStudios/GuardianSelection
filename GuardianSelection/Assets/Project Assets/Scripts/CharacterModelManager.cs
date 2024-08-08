using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Updates and keeps track of what model
/// is currently selected based on character data.
/// </summary>
public class CharacterModelManager : MonoBehaviour, IUpdateOnCharacterChange, IInitializeAfterLoad
{
    public CharacterSelectManager SelectManager { get; set; }
    [SerializeField] private Transform _characterPosition;

    private GameObject currentActiveModel;
    private Dictionary<CharacterData, GameObject> _modelsForCharacter = new();
    
    public void Initialize()
    {
        foreach (CharacterData character in SelectManager.CharacterDataManager.CharacterDataList)
        {
            GameObject model = Instantiate(character.Model, _characterPosition.position, _characterPosition.rotation, transform);
            _modelsForCharacter.Add(character, model);
            model.SetActive(false);
        }
    }
    
    public void HandleCharacterChange(CharacterData data)
    {
        //Deactivate current model
        if(currentActiveModel!=null)
            currentActiveModel.gameObject.SetActive(false);
        
        //Grab the corresponding model and set it active
        _modelsForCharacter[data].gameObject.SetActive(true);
        currentActiveModel = _modelsForCharacter[data];
    }

}