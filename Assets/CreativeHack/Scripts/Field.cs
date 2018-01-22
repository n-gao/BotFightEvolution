using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FieldStatus {
    Free,
    Blocked,
    Character,
    Projectile
}

public class Field {
    public FieldStatus Status {get; set;}
    public int Team;
    public GameObject Occupier = null;

    public Field() {
        Status = FieldStatus.Free;
    }
}
