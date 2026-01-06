import { Alert, Container, Spinner } from 'react-bootstrap';
import { ChatGptAnalysResponseDto } from "../../types/dto/question/correct-answer-dto";
import { useMemo } from 'react';

interface GraphVisualizerProps {
    data?: ChatGptAnalysResponseDto | null;
    isLoading?: boolean;
}

export function GraphVisualizer({ data, isLoading = false }: GraphVisualizerProps) {
    const svgWidth = 400;
    const svgHeight = 400;
    const centerX = svgWidth / 2;
    const centerY = svgHeight / 2;
    const radius = 150;

    const vertexPositions = useMemo(() => {
        if (!data?.Vertexes?.length) return {};

        return data.Vertexes.reduce((acc, vertex, index) => {
            if (vertex?.number == null) return acc;

            const angle = (index * 2 * Math.PI) / data.Vertexes.length;
            acc[vertex.number] = {
                x: centerX + radius * Math.cos(angle),
                y: centerY + radius * Math.sin(angle),
                name: vertex.name || `Node ${vertex.number}`
            };
            return acc;
        }, {} as Record<number, { x: number; y: number; name: string }>);
    }, [data?.Vertexes]);

    if (isLoading) {
        return (
            <Container className="text-center my-5">
                <Spinner animation="border" variant="primary" />
            </Container>
        );
    }

    if (!data?.Vertexes?.length || !data?.Edges?.length) {
        return (
            <Container className="my-3">
                <Alert variant="warning">Данные для графа недоступны или некорректны</Alert>
            </Container>
        );
    }

    return (
        <Container className="my-3">
            <Alert variant="info" className="mb-3">
                Нажмите на любую вершину, чтобы увидеть её подробное описание
            </Alert>

            <div style={{
                border: '1px solid #ddd',
                borderRadius: '8px',
                overflow: 'hidden',
                height: '400px',
                backgroundColor: '#f9f9f9',
                position: 'relative'
            }}>
                <svg width="100%" height="100%" viewBox={`0 0 ${svgWidth} ${svgHeight}`}>
                    <defs>
                        <marker
                            id="arrowhead"
                            markerWidth="10"
                            markerHeight="7"
                            refX="9"
                            refY="3.5"
                            orient="auto"
                        >
                            <polygon points="0 0, 10 3.5, 0 7" fill="#888" />
                        </marker>

                        <filter id="shadow" x="-20%" y="-20%" width="140%" height="140%">
                            <feDropShadow dx="0" dy="2" stdDeviation="2" floodColor="rgba(0,0,0,0.3)" />
                        </filter>
                    </defs>

                    {data.Edges.map((edge, index) => {
                        if (edge?.from == null || edge?.to == null) return null;

                        const fromPos = vertexPositions[edge.from];
                        const toPos = vertexPositions[edge.to];
                        if (!fromPos || !toPos) return null;

                        const dx = toPos.x - fromPos.x;
                        const dy = toPos.y - fromPos.y;
                        const length = Math.sqrt(dx * dx + dy * dy);

                        const nx = dx / length;
                        const ny = dy / length;

                        const fromX = fromPos.x + nx * 20;
                        const fromY = fromPos.y + ny * 20;
                        const toX = toPos.x - nx * 20;
                        const toY = toPos.y - ny * 20;

                        return (
                            <line
                                key={`link-${index}`}
                                x1={fromX}
                                y1={fromY}
                                x2={toX}
                                y2={toY}
                                stroke="#888"
                                strokeWidth="2"
                                markerEnd="url(#arrowhead)"
                            />
                        );
                    })}

                    {data.Vertexes.map((vertex) => {
                        if (vertex?.number == null) return null;
                        const pos = vertexPositions[vertex.number];
                        if (!pos) return null;

                        return (
                            <g
                                key={`vertex-${vertex.number}`}
                                style={{ cursor: 'pointer' }}
                            >
                                <circle
                                    cx={pos.x}
                                    cy={pos.y}
                                    r="20"
                                    fill="#104cac"
                                    stroke="#560a7a"
                                    strokeWidth="2"
                                    filter="url(#shadow)"
                                />
                                <circle
                                    cx={pos.x}
                                    cy={pos.y}
                                    r="16"
                                    fill="#104cac"
                                />
                                <text
                                    x={pos.x}
                                    y={pos.y}
                                    textAnchor="middle"
                                    dy=".3em"
                                    fill="white"
                                    fontSize="10"
                                    fontWeight="bold"
                                >
                                    {vertex.number}
                                </text>
                                <title>{pos.name}</title>
                            </g>
                        );
                    })}
                </svg>
            </div>
        </Container>
    );
}