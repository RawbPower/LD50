using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public float healthPerSlot;
    public int healthSlots;
    public Slider[] healthSliders;

    public float regenRate;
    public float regenAmount;
    public float regenStallFromDamage;

    public ParticleSystem bloodHit;
    public ParticleSystem bloodDeath;

    public bool alwaysShowUI = true;
    public float activeTime;
    public GameObject UIGroup;

    public GameObject destroyOnKill;
    public GameObject[] activateOnKill;

    public Sun sun;

    private int slotsRemaining;
    private float currentSlotHealth;
    private float regenTime;
    private bool UIActive;
    private float activeCounter;

    // Start is called before the first frame update
    void Start()
    {
        FullHeal();
        UIActive = false;
        activeCounter = 0.0f;
    }

    public void Reset()
    {
        FullHeal();
        UIActive = false;
        activeCounter = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (GetTotalHealth() == 0.0f)
        {
            Entity entity = GetComponent<Entity>();
            if (entity != null && entity.IsAlive())
            {
                StartCoroutine(KillEntity());
            }
        }

        if (regenTime <= 0.0f)
        {
            if (regenAmount > 0.0f && currentSlotHealth < healthPerSlot)
            {
                 AddHealthToCurrentSlot(regenAmount);
            }
            regenTime = regenRate;
        }
        else
        {
            regenTime -= Time.deltaTime;
        }

        if (sun != null)
        {
            RemoveHealth(sun.GetSunDamageRate() * Time.deltaTime);
        }

        UpdateHealthSlider();


        Debug.Assert(slotsRemaining >= 0, gameObject.name + " has an invalid amount of slots remaining: " + slotsRemaining);
    }

    void UpdateHealthSlider()
    {
        if (!alwaysShowUI)
        {
            UIActive = activeCounter > 0;
            UIGroup.SetActive(UIActive);

            activeCounter -= Time.deltaTime;
        }

        if (healthSliders.Length == 0)
        {
            return;
        }
        if (healthSliders.Length != healthSlots)
        {
            Debug.Log("Number of health sliders (" + healthSliders.Length + ") is not equal to the number of health slots (" + healthSlots + ")");
            return;
        }

        int fullSlots = slotsRemaining > 0 ? slotsRemaining - 1 : 0;
        for (int i = 0; i < healthSlots; i++)
        {
            if (healthSliders[i] == null)
            {
                Debug.LogError("Health slot " + i + " has an invalid sider");
                return;
            }
            if (slotsRemaining == 0)
            {
                healthSliders[i].value = 0;
            }
            else if (i < fullSlots)
            {
                healthSliders[i].value = healthSliders[i].maxValue;
            }
            else if (i > fullSlots)
            {
                healthSliders[i].value = 0;
            }
            else
            {
                float healthRatio = currentSlotHealth / (healthPerSlot);
                healthSliders[i].value = Mathf.CeilToInt(healthRatio * healthSliders[i].maxValue);
            }
        }
    }

    public IEnumerator KillEntity()
    {
        Entity entity = GetComponent<Entity>();
        if (entity != null)
        {
    
            entity.SetAlive(false);
        }

        if (UIGroup != null)
        {
            UIGroup.SetActive(false);
        }

        if (destroyOnKill != null)
        {
            Destroy(destroyOnKill);
        }

        foreach (GameObject go in activateOnKill)
        {
            go.SetActive(true);
        }

        GarlicKnight garlicKnight = GetComponent<GarlicKnight>();
        if (garlicKnight != null)
        {
            Sun sun = FindObjectOfType<Sun>();
            sun.ResumeTime();
            FindObjectOfType<GlobalAudioManager>().Stop("GarlicKnightTheme");
            FindObjectOfType<GlobalAudioManager>().Play("NightTheme");
        }

        Chariot chariot = GetComponent<Chariot>();
        if (chariot != null)
        {
            yield return new WaitForSeconds(3.0f);
            chariot.winScreen.SetActive(true);
            Player player = FindObjectOfType<Player>();
            player.SetAlive(false);
            FindObjectOfType<GlobalAudioManager>().Stop("Chariot");
            FindObjectOfType<GlobalAudioManager>().Stop("ChariotTheme");
        }

        bloodDeath?.Play();

        if (!gameObject.CompareTag("Player"))
        {
            AnimationManager animationManager = GetComponent<AnimationManager>();
            if (animationManager)
            {
                animationManager.SetBoolParameter("Dead", true);
            }
        }
        else
        {
            AnimationManager animationManager = GetComponent<AnimationManager>();
            if (animationManager)
            {
                animationManager.SetBoolParameter("Dead", true);
            }
            yield return new WaitForSeconds(1.0f);
            Player player = GetComponent<Player>();
            player.endScreen.SetActive(true);
        }
    }

    public void RemoveHealth(float healthToRemove)
    {
        float healthPool = healthToRemove;
        while (healthPool > 0 && slotsRemaining > 0)
        {
            if (healthPool >= currentSlotHealth)
            {
                healthPool -= currentSlotHealth;
                slotsRemaining -= 1;
                currentSlotHealth = healthPerSlot;
            }
            else
            {
                currentSlotHealth -= healthPool;
                healthPool = 0.0f;
            }
        }

        if (GetTotalHealth() <= 0.0f || slotsRemaining == 0)
        {
            slotsRemaining = 0;
            currentSlotHealth = 0.0f;
        }

        regenTime = regenStallFromDamage;

        SetUIActive();
    }

    public void FullHeal()
    {
        slotsRemaining = healthSlots;
        currentSlotHealth = healthPerSlot;
        regenTime = regenRate;
    }

    public void AddHealthToCurrentSlot(float health)
    {
        currentSlotHealth += health;
        currentSlotHealth = Mathf.Min(currentSlotHealth, healthPerSlot);

        SetUIActive();
    }

    public void SetSlotHealth(float slotHealth)
    {
        currentSlotHealth = slotHealth;
        currentSlotHealth = Mathf.Min(currentSlotHealth, healthPerSlot);
    }

    public float GetTotalHealth()
    {
        return Mathf.Max((slotsRemaining - 1), 0.0f) * healthPerSlot + currentSlotHealth;
    }

    public void AddSlot()
    {
        slotsRemaining += 1;
        slotsRemaining = Mathf.Min(slotsRemaining, healthSlots);
        currentSlotHealth = healthPerSlot;
    }

    public void RemoveSlot()
    {
        slotsRemaining -= 1;
        slotsRemaining = Mathf.Max(0, slotsRemaining);
        currentSlotHealth = healthPerSlot;
    }

    public float GetHealthInSlot()
    {
        return currentSlotHealth;
    }

    public int GetHealthSlotsRemaining()
    {
        return slotsRemaining;
    }

    public void SetUIActive()
    {
        if (!alwaysShowUI)
        {
            activeCounter = activeTime;
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Health))]
public class HealthEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Health health = (Health)target;
        if (GUILayout.Button("Full Heal"))
        {
            health.FullHeal();
        }
    }
}
#endif
