# SPDX-FileCopyrightText: 2025 MirageEexe <Mirageeexe@gmail.com>
# SPDX-License-Identifier: AGPL-3.0-or-later

namespace Content.Goobstation.Shared.MartialArts.Events;

// Залом руки с дизармом: Disarm → Harm
public sealed class ArbArmLockPerformedEvent : EntityEventArgs;

// Подсечка стоя с опрокидыванием: Harm → Harm → Disarm
public sealed class ArbSweepPerformedEvent : EntityEventArgs;

// Удушающий стоя в партере: Disarm → Harm → Harm
public sealed class ArbChokePerformedEvent : EntityEventArgs;

// Удар локтём: Harm → Harm
public sealed class ArbElbowStrikePerformedEvent : EntityEventArgs;

// Удар коленом: Harm → Disarm
public sealed class ArbKneeStrikePerformedEvent : EntityEventArgs;

// Бросок через бедро: Disarm → Harm → Disarm
public sealed class ArbHipThrowPerformedEvent : EntityEventArgs;
