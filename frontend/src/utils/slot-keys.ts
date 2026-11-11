export const SlotKeys = {
    pocket: 'POCKET',
    rightHand: 'HAND_RIGHT',
    leftHand: 'HAND_LEFT',
} as const;

export const handSlotKeys: readonly string[] = [SlotKeys.rightHand, SlotKeys.leftHand];

export function isPocketSlot(slotKey: string): boolean {
    return slotKey === SlotKeys.pocket;
}

export function isHandSlot(slotKey: string): boolean {
    return handSlotKeys.includes(slotKey);
}

export function isBodySlot(slotKey: string): boolean {
    return slotKey.includes('_BODY_');
}
