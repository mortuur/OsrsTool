import { useState, useEffect, useCallback } from 'react';
import { CombatStyle, CombatStyleNames, GearSlotNames } from '../types/gear';
import type { GearUpgradeDto, GearSlot } from '../types/gear';
import './GearUpgradeCalculator.css';

const GearUpgradeCalculator = () => {
  const [combatStyle, setCombatStyle] = useState<CombatStyle>(CombatStyle.Melee);
  const [upgrades, setUpgrades] = useState<GearUpgradeDto[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [showNetCost, setShowNetCost] = useState(true);

  const fetchUpgrades = useCallback(async () => {
    setLoading(true);
    setError(null);
    
    try {
      const response = await fetch('/api/gear/upgrades', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          combatStyle,
          ownedItemIds: [],
        }),
      });

      if (!response.ok) {
        throw new Error('Failed to fetch upgrades');
      }

      const data = await response.json();
      setUpgrades(data);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'An error occurred');
    } finally {
      setLoading(false);
    }
  }, [combatStyle]);

  useEffect(() => {
    fetchUpgrades();
  }, [fetchUpgrades]);

  const formatPrice = (price: number): string => {
    if (price >= 1000000) {
      return `${(price / 1000000).toFixed(1)}M gp`;
    } else if (price >= 1000) {
      return `${(price / 1000).toFixed(0)}K gp`;
    }
    return `${price} gp`;
  };

  const getSlotName = (slot: GearSlot): string => {
    return GearSlotNames[slot];
  };

  const getCombatStyleName = (style: CombatStyle): string => {
    return CombatStyleNames[style];
  };

  return (
    <div className="gear-calculator">
      <h1>OSRS Gear Upgrade Calculator</h1>
      
      <div className="controls">
        <div className="combat-style-selector">
          <h2>Select Combat Style</h2>
          <div className="style-buttons">
            <button
              className={combatStyle === CombatStyle.Melee ? 'active' : ''}
              onClick={() => setCombatStyle(CombatStyle.Melee)}
            >
              ⚔️ Melee
            </button>
            <button
              className={combatStyle === CombatStyle.Range ? 'active' : ''}
              onClick={() => setCombatStyle(CombatStyle.Range)}
            >
              🏹 Range
            </button>
            <button
              className={combatStyle === CombatStyle.Mage ? 'active' : ''}
              onClick={() => setCombatStyle(CombatStyle.Mage)}
            >
              🔮 Mage
            </button>
          </div>
        </div>

        <div className="cost-toggle">
          <label>
            <input
              type="checkbox"
              checked={showNetCost}
              onChange={(e) => setShowNetCost(e.target.checked)}
            />
            Consider selling current item
          </label>
        </div>
      </div>

      {loading && <div className="loading">Loading upgrades...</div>}
      {error && <div className="error">{error}</div>}

      {!loading && !error && (
        <div className="upgrades-list">
          <h2>Recommended Upgrades for {getCombatStyleName(combatStyle)}</h2>
          
          {upgrades.length === 0 ? (
            <p className="no-upgrades">
              You have all the best-in-slot gear! Congratulations! 🎉
            </p>
          ) : (
            <div className="upgrade-cards">
              {upgrades.map((upgrade, index) => (
                <div key={index} className="upgrade-card">
                  <div className="slot-header">
                    <h3>{getSlotName(upgrade.slot)}</h3>
                  </div>
                  
                  <div className="upgrade-content">
                    <div className="current-item">
                      <div className="item-label">Current:</div>
                      <div className="item-details">
                        {upgrade.currentItem ? (
                          <>
                            <div className="item-name">{upgrade.currentItem.name}</div>
                            <div className="item-price">
                              {upgrade.currentItem.currentPrice 
                                ? formatPrice(upgrade.currentItem.currentPrice)
                                : 'Price unavailable'}
                            </div>
                          </>
                        ) : (
                          <div className="item-name">No item equipped</div>
                        )}
                      </div>
                    </div>

                    <div className="upgrade-arrow">→</div>

                    <div className="next-item">
                      <div className="item-label">Next Upgrade:</div>
                      <div className="item-details">
                        {upgrade.nextUpgrade && (
                          <>
                            <div className="item-name">{upgrade.nextUpgrade.name}</div>
                            <div className="item-price">
                              {upgrade.nextUpgrade.currentPrice 
                                ? formatPrice(upgrade.nextUpgrade.currentPrice)
                                : 'Price unavailable'}
                            </div>
                          </>
                        )}
                      </div>
                    </div>
                  </div>

                  <div className="cost-summary">
                    <div className="cost-label">
                      {showNetCost && upgrade.currentItem 
                        ? 'Net Cost (after selling):' 
                        : 'Upgrade Cost:'}
                    </div>
                    <div className={`cost-value ${(showNetCost ? upgrade.netCost : upgrade.upgradeCost) < 0 ? 'profit' : ''}`}>
                      {formatPrice(Math.abs(showNetCost ? upgrade.netCost : upgrade.upgradeCost))}
                      {showNetCost && upgrade.netCost < 0 && ' profit!'}
                    </div>
                  </div>
                </div>
              ))}
            </div>
          )}
        </div>
      )}
    </div>
  );
};

export default GearUpgradeCalculator;
