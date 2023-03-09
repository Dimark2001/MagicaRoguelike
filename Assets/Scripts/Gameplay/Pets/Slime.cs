using UnityEngine;

public class Slime : Pets
{
    protected override void UseAbility(Transform enemy)
    {
        base.UseAbility(enemy);
        var rain = Instantiate(AbilityManager.Instance.meteoriteRain, enemy.transform.position, Quaternion.identity);
        rain.GetComponent<MeteoriteRain>().CreateMeteoriteRain(50, 4, 4);
        var vfx = Instantiate(AbilityManager.Instance.vfxMeteoriteRain);
        vfx.transform.position = enemy.position;
        vfx.transform.localScale = new Vector3(1, 1, 1) * 4 / 4;
    }
}
