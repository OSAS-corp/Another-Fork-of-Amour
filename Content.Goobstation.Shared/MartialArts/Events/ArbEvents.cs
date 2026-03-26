# SPDX-FileCopyrightText: 2025 MirageEexe <Mirageeexe@gmail.com>
# SPDX-License-Identifier: AGPL-3.0-or-later

namespace Content.Goobstation.Shared.MartialArts.Events;

// Залом руки с дизармом
public sealed class ArbArmLockPerformedEvent : EntityEventArgs;

// Подсечка стоя с опрокидыванием
public sealed class ArbSweepPerformedEvent : EntityEventArgs;

// Удушающий стоя в партере
public sealed class ArbChokePerformedEvent : EntityEventArgs;

// Удар локтём
public sealed class ArbElbowStrikePerformedEvent : EntityEventArgs;

// Удар коленом
public sealed class ArbKneeStrikePerformedEvent : EntityEventArgs;

// Бросок через бедро
public sealed class ArbHipThrowPerformedEvent : EntityEventArgs;
