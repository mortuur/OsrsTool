# OSRS Gear Upgrade Calculator

## Overview
This feature allows users to easily view and calculate the cost of their next gear upgrade for all three combat styles in Old School RuneScape (OSRS): Melee, Range, and Mage.

## Features

### User Capabilities
- **Combat Style Selection**: Choose between Melee, Range, or Mage combat styles
- **Upgrade Path Visualization**: See the complete progression path from starter gear to best-in-slot (BIS)
- **Cost Calculations**: View both:
  - **Upgrade Cost**: The full price of the next upgrade
  - **Net Cost**: The cost after selling your current item (if applicable)
- **Real-time Pricing**: Prices are fetched from the database when available
- **Responsive Design**: Works on desktop and mobile devices

### Technical Implementation

#### Backend (C# / .NET 9)
- **Domain Models**:
  - `CombatStyle` enum: Melee, Range, Mage
  - `GearSlot` enum: Head, Body, Legs, Weapon, Cape, Neck, Hands, Feet, Ring, Shield, Ammo
  - `GearItemDto`: Represents a gear item with pricing
  - `GearUpgradeDto`: Represents an upgrade path with cost calculations

- **Services**:
  - `IGearService`: Service interface for gear operations
  - `GearService`: Implementation with BIS progression logic

- **API Endpoints**:
  - `POST /api/gear/upgrades`: Get upgrade recommendations based on owned items
  - `GET /api/gear/bis/{combatStyle}`: Get complete BIS progression for a combat style

- **Data**:
  - `BisGearData.json`: Comprehensive gear progression data for all combat styles
  - Includes accurate OSRS item IDs and progression tiers

#### Frontend (React / TypeScript)
- **Components**:
  - `GearUpgradeCalculator`: Main component with combat style selector and upgrade display
  
- **Features**:
  - Dark theme matching OSRS aesthetic
  - Responsive card layout for each gear slot
  - Toggle for net cost vs upgrade cost
  - Loading and error state handling

## Usage

### For Users
1. Navigate to the gear upgrade calculator
2. Select your combat style (Melee, Range, or Mage)
3. View recommended upgrades sorted by cost
4. Toggle "Consider selling current item" to see net costs
5. Each card shows:
   - Current equipped item (if any)
   - Next recommended upgrade
   - Cost information

### For Developers

#### Adding New Gear
To add new BIS gear to the progression:

1. Edit `/OsrsTool.server/Data/BisGearData.json`
2. Add the item to the appropriate combat style and slot
3. Ensure the item has:
   - `itemId`: Correct OSRS item ID
   - `name`: Item name
   - `tier`: Progression tier (higher = better)

Example:
```json
{
  "tier": 8,
  "itemId": 22322,
  "name": "Torva full helm"
}
```

#### Testing
Run the test suite:
```bash
dotnet test
```

The test suite includes:
- BIS progression retrieval tests
- Upgrade calculation with and without owned items
- Tier progression logic verification

## Architecture

```
Frontend (React/TypeScript)
    ↓
API Controller (GearController)
    ↓
Service Layer (GearService)
    ↓
Domain Models & DTOs
    ↓
BIS Data (JSON) + Database (Item Prices)
```

## Future Enhancements
Potential improvements:
- User accounts to save owned items
- Price history tracking
- Optimal upgrade path suggestions
- Budget-based recommendations
- Alternative gear setups (e.g., budget vs BIS)

## Notes
- Item prices are fetched from the database when available
- The gear progression is based on common OSRS BIS setups
- Tier ordering reflects general progression but may vary based on use case
