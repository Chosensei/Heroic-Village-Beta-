using UnityEngine;
using UnityEngine.UI;


public class AbilitiesCDTimer : MonoBehaviour
{
    [Header("Ability 1")]
    [SerializeField] Image skillImage1;
    [SerializeField] float cdTime1 = 5;
    private bool isCooldown1 = false;

    // Ability 1 Input Variables 
    private Vector3 position;
    [SerializeField] Canvas ability1Canvas;
    [SerializeField] Image skillshot;
    [SerializeField] Transform player; 


    [Header("Ability 2")]
    [SerializeField] Image skillImage2;
    [SerializeField] float cdTime2 = 5;
    private bool isCooldown2 = false;

    // Ability 2 Input Variables 
    private Vector3 positionUp;
    [SerializeField] Canvas ability2Canvas;
    [SerializeField] Image targetCircle;
    [SerializeField] Image indicatorRangeCircle;
    [SerializeField] float maxAbilityToDistance;


    [Header("Ability 3")]
    [SerializeField] Image skillImage3;
    [SerializeField] float cdTime3 = 5;
    private bool isCooldown3 = false;


    [Header("Ability 4")]
    [SerializeField] Image skillImage4;
    [SerializeField] float cdTime4 = 5;
    private bool isCooldown4 = false;

    [SerializeField] MagicData[] _md;

    private void Start()
    {
        skillImage1.fillAmount = 0;
        skillImage2.fillAmount = 0;
        skillImage3.fillAmount = 0;
        skillImage4.fillAmount = 0;

        // for scriptable objects magic
        cdTime1 = _md[0].CastTime; 
        cdTime2 = _md[1].CastTime;
        cdTime3 = _md[2].CastTime;

        // special case magic
        cdTime4 = 30f;

        skillshot.GetComponent<Image>().enabled = false;
        targetCircle.GetComponent<Image>().enabled = false;
        indicatorRangeCircle.GetComponent<Image>().enabled = false;

    }

    private void Update()
    {
        // TOFIX: CD TImer runs regardless of whether the target is in range or the spell is fired
        Ability1CDTimer();
        Ability2CDTimer();
        Ability3CDTimer();
        Ability4CDTimer();

        RaycastHit hit; 
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Ability 1 Inputs
        //if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        //{
        //    position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
        //}

        // Ability 2 Inputs
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject != this.gameObject)
            {
                positionUp = new Vector3(hit.point.x, 10f, hit.point.z);
                position = hit.point;
            }
        }

        //// Ability 1 Canvas Inputs
        //Quaternion transRot = Quaternion.LookRotation(position - player.transform.position);
        //transRot.eulerAngles = new Vector3(0, transRot.eulerAngles.y, transRot.eulerAngles.z);
        //ability1Canvas.transform.rotation = Quaternion.Lerp(transRot, ability1Canvas.transform.rotation, 0f);

        //Ability 2 Canvas Inputs
        var hitPosDir = (hit.point - transform.position).normalized;
        float distance = Vector3.Distance(hit.point, transform.position);
        distance = Mathf.Min(distance, maxAbilityToDistance);

        var newHitPos = transform.position + hitPosDir * distance;
        ability2Canvas.transform.position = (newHitPos);
    }
    public void Ability1CDTimer()
    {
        //if (Input.GetKey(UIManager.Instance.ActionSkill_01) && !isCooldown1)
        if (Input.GetKeyDown(KeyCode.Z) && !isCooldown1)
        {
            // enable skilshot
            skillshot.GetComponent<Image>().enabled = true;
            // disable other skill UI
            targetCircle.GetComponent<Image>().enabled = false;
            indicatorRangeCircle.GetComponent<Image>().enabled = false;
        }
        if (skillshot.GetComponent<Image>().enabled == true && Input.GetMouseButton(0))
        {
            isCooldown1 = true;
            skillImage1.fillAmount = 1;
        }
        if (isCooldown1)
        {
            skillImage1.fillAmount -= 1 / cdTime1 * Time.deltaTime;

            skillshot.GetComponent<Image>().enabled = false;

            if (skillImage1.fillAmount <= 0)
            {
                skillImage1.fillAmount = 0;
                isCooldown1 = false; 
            }
        }
    }
    public void Ability2CDTimer()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !isCooldown2)
        {
            indicatorRangeCircle.GetComponent<Image>().enabled = true;
            targetCircle.GetComponent<Image>().enabled = true;

            //Disable Skillshot UI
            skillshot.GetComponent<Image>().enabled = false;
        }

        if (targetCircle.GetComponent<Image>().enabled == true && Input.GetMouseButtonDown(0))
        {
            isCooldown2 = true;
            skillImage2.fillAmount = 1;
        }
        if (isCooldown2)
        {
            skillImage2.fillAmount -= 1 / cdTime2 * Time.deltaTime;

            indicatorRangeCircle.GetComponent<Image>().enabled = false;
            targetCircle.GetComponent<Image>().enabled = false;

            if (skillImage2.fillAmount <= 0)
            {
                skillImage2.fillAmount = 0;
                isCooldown2 = false;
            }
        }
    }
    public void Ability3CDTimer()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !isCooldown3)
        {
            isCooldown3 = true;
            skillImage3.fillAmount = 1;
        }
        if (isCooldown3)
        {
            skillImage3.fillAmount -= 1 / cdTime3 * Time.deltaTime;
            if (skillImage3.fillAmount <= 0)
            {
                skillImage3.fillAmount = 0;
                isCooldown3 = false;
            }
        }
    }
    public void Ability4CDTimer()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !isCooldown4)
        {
            isCooldown4 = true;
            skillImage4.fillAmount = 1;
        }
        if (isCooldown4)
        {
            skillImage4.fillAmount -= 1 / cdTime4 * Time.deltaTime;
            if (skillImage4.fillAmount <= 0)
            {
                skillImage4.fillAmount = 0;
                isCooldown4 = false;
            }
        }
    }
}
