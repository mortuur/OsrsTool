export const CombatStyle = {
  Melee: 0,
  Range: 1,
  Mage: 2
} as const;

export type CombatStyle = typeof CombatStyle[keyof typeof CombatStyle];

export const CombatStyleNames: Record<CombatStyle, string> = {
  [CombatStyle.Melee]: 'Melee',
  [CombatStyle.Range]: 'Range',
  [CombatStyle.Mage]: 'Mage'
};

export const GearSlot = {
  Head: 0,
  Cape: 1,
  Neck: 2,
  Ammo: 3,
  Weapon: 4,
  Body: 5,
  Shield: 6,
  Legs: 7,
  Hands: 8,
  Feet: 9,
  Ring: 10
} as const;

export type GearSlot = typeof GearSlot[keyof typeof GearSlot];

export const GearSlotNames: Record<GearSlot, string> = {
  [GearSlot.Head]: 'Head',
  [GearSlot.Cape]: 'Cape',
  [GearSlot.Neck]: 'Neck',
  [GearSlot.Ammo]: 'Ammo',
  [GearSlot.Weapon]: 'Weapon',
  [GearSlot.Body]: 'Body',
  [GearSlot.Shield]: 'Shield',
  [GearSlot.Legs]: 'Legs',
  [GearSlot.Hands]: 'Hands',
  [GearSlot.Feet]: 'Feet',
  [GearSlot.Ring]: 'Ring'
};

export interface GearItemDto {
  itemId: number;
  name: string;
  slot: GearSlot;
  tier: number;
  currentPrice: number | null;
}

export interface GearUpgradeDto {
  combatStyle: CombatStyle;
  slot: GearSlot;
  currentItem: GearItemDto | null;
  nextUpgrade: GearItemDto | null;
  upgradeCost: number;
  netCost: number;
}

export interface GearProgressionRequestDto {
  combatStyle: CombatStyle;
  ownedItemIds: number[];
}
