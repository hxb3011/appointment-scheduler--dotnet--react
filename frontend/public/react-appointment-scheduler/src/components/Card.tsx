import * as React from "react";
import styled from "styled-components";

function MyComponent() {
  return (
    <Deviceframe>
      <BuildingBlocksstatusbar>
        <Time>9:30</Time>
        <Img
          loading="lazy"
          src="https://cdn.builder.io/api/v1/image/assets/TEMP/732dd75d657d520fc5e594f705482d97773d831b3aa75092ea60477269c5226f?placeholderIfAbsent=true&apiKey=6095c023a0564c1099f3473962fff0f7"
        />
        <Img2
          loading="lazy"
          src="https://cdn.builder.io/api/v1/image/assets/TEMP/596f03ddd92a57478ccf57b7ecc89b6ccd056bf32196d03c1e64ce4229222e84?placeholderIfAbsent=true&apiKey=6095c023a0564c1099f3473962fff0f7"
        />
      </BuildingBlocksstatusbar>
      <Handle />
    </Deviceframe>
  );
}

const Deviceframe = styled.div`
  justify-content: center;
  border-radius: 18px;
  border: 8px solid var(--Schemes-Outline-Variant, #cac4d0);
  background: var(--Schemes-Surface-Container-Lowest, #fff);
  background-color: var(--Schemes-Surface-Container-Lowest, #fff);
  display: flex;
  max-width: 480px;
  width: 100%;
  flex-direction: column;
  overflow: hidden;
  color: var(--M3-sys-light-on-surface, var(--Schemes-On-Surface, #1d1b20));
  white-space: nowrap;
  letter-spacing: 0.14px;
  margin: 0 auto;
  padding: 8px 8px 18px;
  font: 500 14px/1 Roboto, sans-serif;
`;

const BuildingBlocksstatusbar = styled.div`
  position: relative;
  display: flex;
  min-height: 52px;
  align-items: end;
  justify-content: space-between;
  padding: 22px 24px 10px;
`;

const Time = styled.div`
  font-variant-numeric: lining-nums proportional-nums;
  font-feature-settings: "dlig" on, "ss02" on;
  z-index: 0;
`;

const Img = styled.img`
  aspect-ratio: 2.7;
  object-fit: contain;
  object-position: center;
  width: 46px;
  z-index: 0;
`;

const Img2 = styled.img`
  aspect-ratio: 1;
  object-fit: contain;
  object-position: center;
  width: 24px;
  fill: var(--Schemes-On-Surface, #1d1b20);
  align-self: start;
  position: absolute;
  z-index: 0;
  left: 50%;
  bottom: 10px;
  transform: translate(-50%, 0%);
  height: 24px;
`;

const Handle = styled.div`
  border-radius: 12px;
  background: var(--Schemes-On-Surface, #1d1b20);
  background-color: var(--Schemes-On-Surface, #1d1b20);
  align-self: center;
  display: flex;
  margin-top: 826px;
  width: 108px;
  height: 4px;
`;